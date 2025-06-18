using Hotel_Management.Areas.Admin.Services.Interfaces;
using Hotel_Management.Areas.Admin.ViewModels;
using Hotel_Management.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hotel_Management.Areas.Admin.Services
{
    public class DashboardServices : IDashboardServices
    {
        private readonly HotelManagementContext _context;
        private readonly ILogger<DashboardServices> _logger;

        public DashboardServices(HotelManagementContext context, ILogger<DashboardServices> logger)
        {
            _context = context;
            _logger = logger;
        }

        #region Summary
        public int GetTotalBookings(DateTime startDate, DateTime endDate)
        {
            try
            {
                // Đảm bảo endDate bao gồm cả ngày cuối
                var adjustedEndDate = endDate.Date.AddDays(1).AddSeconds(-1);

                // Lấy tổng số đặt phòng trong khoảng thời gian hiện tại
                var currentTotal = _context.Bookings
                    .Where(b => b.CreatedAt >= startDate.Date && b.CreatedAt <= adjustedEndDate)
                    .Count();

                return currentTotal;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy tổng số đặt phòng");
                throw new DashboardServiceException("Không thể lấy dữ liệu tổng số đặt phòng", ex);
            }
        }

        public int GetTotalCustomers()
        {
            try
            {
                // Lấy tổng số khách hàng bằng cách truy vấn bảng UserRoles và Roles
                // trong Identity framework thay vì dựa vào NormalizedUserName
                var customerRoleId = _context.Roles
                    .Where(r => r.NormalizedName == "CUSTOMER")
                    .Select(r => r.Id)
                    .FirstOrDefault();

                if (string.IsNullOrEmpty(customerRoleId))
                {
                    _logger.LogWarning("Không tìm thấy role Customer trong hệ thống");
                    return 0;
                }

                var totalCustomers = _context.UserRoles
                    .Where(ur => ur.RoleId == customerRoleId)
                    .Count();

                // Nếu không có khách hàng, trả về 0
                if (totalCustomers <= 0)
                {
                    _logger.LogWarning("Không tìm thấy khách hàng nào trong hệ thống");
                    return 0;
                }

                return totalCustomers;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy tổng số khách hàng");
                // Return 0 instead of throwing to maintain resilience
                return 0;
            }
        }

        public decimal GetTotalServiceRevenue(DateTime startDate, DateTime endDate)
        {
            try
            {
                // Đảm bảo endDate bao gồm cả ngày cuối
                var adjustedEndDate = endDate.Date.AddDays(1).AddSeconds(-1);

                // Lấy tổng doanh thu dịch vụ trong khoảng thời gian hiện tại
                var currentTotal = _context.Payments
                    .Where(p => p.Status == "Success" &&
                                p.PaymentDate >= startDate.Date &&
                                p.PaymentDate <= adjustedEndDate)
                    .Join(_context.Bookings,
                        p => p.BookingId,
                        b => b.Id,
                        (p, b) => new
                        {
                            Payment = p,
                            Booking = b
                        })
                    .Sum(x => x.Booking.TotalPriceServices ?? 0);

                return currentTotal;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy tổng doanh thu dịch vụ");
                throw new DashboardServiceException("Không thể lấy dữ liệu tổng doanh thu dịch vụ", ex);
            }
        }

        public decimal GetTotalRoomRevenue(DateTime startDate, DateTime endDate)
        {
            try
            {
                // Đảm bảo endDate bao gồm cả ngày cuối
                var adjustedEndDate = endDate.Date.AddDays(1).AddSeconds(-1);

                // Lấy tổng doanh thu phòng trong khoảng thời gian hiện tại
                var currentTotal = _context.Payments
                    .Where(p => p.Status == "Success" &&
                                p.PaymentDate >= startDate.Date &&
                                p.PaymentDate <= adjustedEndDate)
                    .Join(_context.Bookings,
                        p => p.BookingId,
                        b => b.Id,
                        (p, b) => new
                        {
                            Payment = p,
                            Booking = b
                        })
                    .Sum(x => x.Booking.TotalPriceRooms ?? 0);

                return currentTotal;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy tổng doanh thu phòng");
                throw new DashboardServiceException("Không thể lấy dữ liệu tổng doanh thu phòng", ex);
            }
        }

        #endregion

        #region Chart
        public List<DashboardVM.ChartDataPoint> GetRevenueChartData(DateTime startDate, DateTime endDate)
        {
            try
            {
                var result = new List<DashboardVM.ChartDataPoint>();

                // Adjust the end date to include the entire day
                var adjustedEndDate = endDate.Date.AddDays(1).AddSeconds(-1);

                // First, collect all needed data in memory to avoid multiple database queries
                // This prevents thread safety issues with DbContext
                var revenueByDay = _context.Payments
                    .AsNoTracking()
                    .Where(p => p.Status == "Success" &&
                                p.PaymentDate.HasValue &&
                                p.PaymentDate.Value >= startDate.Date &&
                                p.PaymentDate.Value <= adjustedEndDate)
                    .Join(_context.Bookings,
                          p => p.BookingId,
                          b => b.Id,
                          (p, b) => new
                          {
                              Date = p.PaymentDate.Value.Date,
                              Revenue = b.TotalPrice ?? 0
                          })
                    .GroupBy(x => x.Date)
                    .Select(g => new
                    {
                        Date = g.Key,
                        Revenue = g.Sum(x => x.Revenue)
                    })
                    .OrderBy(x => x.Date)
                    .ToDictionary(x => x.Date, x => x.Revenue);

                // Log warning if no data found
                if (!revenueByDay.Any())
                {
                    _logger.LogWarning("No revenue data found for the date range {StartDate} to {EndDate}",
                        startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"));
                }

                // Create data points for each date (filling in gaps with zeros)
                for (var date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
                {
                    decimal revenue = 0;
                    if (revenueByDay.TryGetValue(date, out var dailyRevenue))
                    {
                        revenue = dailyRevenue;
                    }

                    result.Add(new DashboardVM.ChartDataPoint
                    {
                        Label = date.ToString("dd/MM"),
                        Value = Math.Round(revenue / 1000000, 1)
                    });
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving revenue chart data");
                return new List<DashboardVM.ChartDataPoint>();
            }
        }

        public List<DashboardVM.ChartDataPoint> GetBookingChartData(DateTime startDate, DateTime endDate)
        {
            try
            {
                var result = new List<DashboardVM.ChartDataPoint>();

                // Adjust the end date to include the entire day
                var adjustedEndDate = endDate.Date.AddDays(1).AddSeconds(-1);

                // Get all booking dates within the range
                var bookingDates = _context.Bookings
                    .AsNoTracking()
                    .Where(b => b.CreatedAt >= startDate.Date && b.CreatedAt <= adjustedEndDate)
                    .Join(_context.Payments,
                        b => b.Id,
                        p => p.BookingId,
                        (b, p) => new { Booking = b, Payment = p })
                    .Where(bp => bp.Payment.Status == "Success")
                    .GroupBy(bp => bp.Booking.CreatedAt.Value.Date)
                    .Select(g => new
                    {
                        Date = g.Key,
                        Count = g.Count()
                    })
                    .OrderBy(x => x.Date)
                    .ToDictionary(x => x.Date, x => x.Count);

                // Log warning if no data found
                if (!bookingDates.Any())
                {
                    _logger.LogWarning("No booking data found for the date range {StartDate} to {EndDate}",
                        startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"));
                }

                // Create data points for each date (filling in gaps with zeros)
                for (var date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
                {
                    int bookingCount = 0;
                    if (bookingDates.TryGetValue(date, out var count))
                    {
                        bookingCount = count;
                    }

                    result.Add(new DashboardVM.ChartDataPoint
                    {
                        Label = date.ToString("dd/MM"),
                        Value = bookingCount
                    });
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving booking chart data");
                return new List<DashboardVM.ChartDataPoint>();
            }
        }


        public List<DashboardVM.ChartDataPoint> GetRoomTypeChartData(DateTime startDate, DateTime endDate)
        {
            try
            {
                // Đảm bảo endDate bao gồm cả ngày cuối
                var adjustedEndDate = endDate.Date.AddDays(1).AddSeconds(-1);

                // Lấy tất cả các loại phòng hiện có trong hệ thống
                var allRoomTypes = _context.Rooms
                    .AsNoTracking()
                    .Select(r => r.Type ?? "Unknown Room Type")
                    .Distinct()
                    .ToList();

                var successfulBookingIds = _context.Payments
                    .AsNoTracking()
                    .Where(p => p.Status == "Success")
                    .Select(p => p.BookingId)
                    .Distinct()
                    .ToList();

                // Lấy tổng số đặt phòng
                var totalBookings = _context.BookingsRoomDetails
                    .AsNoTracking()
                    .Include(brd => brd.Room)
                    .Include(brd => brd.Booking)
                    .Where(brd => brd.Booking.CreatedAt >= startDate.Date &&
                                brd.Booking.CreatedAt <= adjustedEndDate &&
                                successfulBookingIds.Contains(brd.BookingId))
                    .Count();

                // Nếu không có booking nào, trả về dữ liệu với tất cả phòng là 0%
                if (totalBookings == 0)
                {
                    _logger.LogWarning("No bookings found for the date range {StartDate} to {EndDate}",
                        startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"));

                    return allRoomTypes.Select(roomType => new DashboardVM.ChartDataPoint
                    {
                        Label = roomType,
                        Value = 0
                    }).ToList();
                }

                // Nhóm theo loại phòng và đếm số lượt đặt
                var roomTypeBookings = _context.BookingsRoomDetails
                    .AsNoTracking()
                    .Include(brd => brd.Room)
                    .Include(brd => brd.Booking)
                    .Where(brd => brd.Booking.CreatedAt >= startDate.Date &&
                                brd.Booking.CreatedAt <= adjustedEndDate &&
                                brd.Room != null &&
                                successfulBookingIds.Contains(brd.BookingId))
                    .GroupBy(brd => brd.Room.Type)
                    .ToDictionary(g => g.Key ?? "Unknown Room Type", g => g.Count());

                // Tạo kết quả bao gồm tất cả các loại phòng, kể cả loại không có đặt phòng
                var result = allRoomTypes.Select(roomType =>
                {
                    // Lấy số lượng booking cho loại phòng, hoặc 0 nếu không có
                    int bookingCount = 0;
                    if (roomTypeBookings.ContainsKey(roomType))
                    {
                        bookingCount = roomTypeBookings[roomType];
                    }

                    // Tính phần trăm
                    decimal percentage = totalBookings > 0
                        ? Math.Round((decimal)bookingCount / totalBookings * 100, 0)
                        : 0;

                    return new DashboardVM.ChartDataPoint
                    {
                        Label = roomType,
                        Value = percentage
                    };
                }).ToList();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy dữ liệu biểu đồ loại phòng: {ErrorMessage}", ex.Message);
                return new List<DashboardVM.ChartDataPoint>();
            }
        }


        public List<DashboardVM.ServiceRevenueItem> GetServiceRevenueItems(DateTime startDate, DateTime endDate)
        {
            try
            {
                // Đảm bảo endDate bao gồm cả ngày cuối
                var adjustedEndDate = endDate.Date.AddDays(1).AddSeconds(-1);

                var allServices = _context.Services
                    .AsNoTracking()
                    .Select(s => s.Name ?? "Unknown Service")
                    .Distinct()
                    .ToList();

                // Lấy danh sách booking id có payment status = Success
                var successfulBookingIds = _context.Payments
                    .AsNoTracking()
                    .Where(p => p.Status == "Success")
                    .Select(p => p.BookingId)
                    .Distinct()
                    .ToList();

                var serviceRevenues = _context.BookingsServiceDetails
                    .AsNoTracking()
                    .Where(bsd => bsd.Booking != null &&
                                bsd.Booking.CreatedAt.HasValue &&
                                bsd.Booking.CreatedAt.Value >= startDate.Date &&
                                bsd.Booking.CreatedAt.Value <= adjustedEndDate &&
                                bsd.Service != null &&
                                successfulBookingIds.Contains(bsd.BookingId))
                    .Select(bsd => new
                    {
                        ServiceName = bsd.Service.Name ?? "Unknown Service",
                        Price = bsd.Price ?? 0
                    })
                    .ToList()
                    .GroupBy(x => x.ServiceName)
                    .ToDictionary(g => g.Key, g => g.Sum(x => x.Price));

                var result = allServices.Select(serviceName => new DashboardVM.ServiceRevenueItem
                    {
                        ServiceName = serviceName,
                        Revenue = serviceRevenues.ContainsKey(serviceName) ? serviceRevenues[serviceName] : 0
                    })
                .OrderByDescending(item => item.Revenue)
                .ToList();

                if (!result.Any())
                {
                    _logger.LogInformation("No service revenue data found for the date range: {StartDate} to {EndDate}",
                        startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"));
                    return new List<DashboardVM.ServiceRevenueItem>();
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách doanh thu dịch vụ");
                return new List<DashboardVM.ServiceRevenueItem>();
            }
        }
        public List<DashboardVM.RoomTypeRevenueItem> GetRoomTypeRevenueItems(DateTime startDate, DateTime endDate)
        {
            try
            {
                var adjustedEndDate = endDate.Date.AddDays(1).AddSeconds(-1);

                // Lấy tất cả các loại phòng hiện có trong hệ thống
                var allRoomTypes = _context.Rooms
                    .AsNoTracking()
                    .Select(r => r.Type ?? "Unknown Room Type")
                    .Distinct()
                    .ToList();

                var successfulBookingIds = _context.Payments
                    .AsNoTracking()
                    .Where(p => p.Status == "Success")
                    .Select(p => p.BookingId)
                    .Distinct()
                    .ToList();

                // Lấy doanh thu từng loại phòng trong kỳ hiện tại
                var roomTypeRevenues = _context.BookingsRoomDetails
                    .AsNoTracking()
                    .Where(brd => brd.Booking != null &&
                                brd.Booking.CreatedAt.HasValue &&
                                brd.Booking.CreatedAt.Value >= startDate.Date &&
                                brd.Booking.CreatedAt.Value <= adjustedEndDate &&
                                brd.Room != null &&
                                successfulBookingIds.Contains(brd.BookingId))
                    .Select(brd => new
                    {
                        RoomType = brd.Room.Type ?? "Unknown Room Type",
                        Price = brd.Price ?? 0
                    })
                    .ToList()
                    .GroupBy(x => x.RoomType)
                    .ToDictionary(g => g.Key, g => g.Sum(x => x.Price));

                // Tạo kết quả bao gồm tất cả các loại phòng, kể cả những loại không có doanh thu
                var result = allRoomTypes.Select(roomType => new DashboardVM.RoomTypeRevenueItem
                    {
                        RoomTypeName = roomType,
                        Revenue = roomTypeRevenues.ContainsKey(roomType) ? roomTypeRevenues[roomType] : 0
                    })
                    .OrderByDescending(item => item.Revenue)
                    .ToList();

                if (!result.Any())
                {
                    _logger.LogInformation("No room type revenue data found for the date range: {StartDate} to {EndDate}",
                        startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"));
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách doanh thu loại phòng");
                return new List<DashboardVM.RoomTypeRevenueItem>();
            }
        }

        public List<DashboardVM.UserReview> GetUserReviews(DateTime startDate, DateTime endDate)
        {
            try
            {
                var adjustedEndDate = endDate.Date.AddDays(1).AddSeconds(-1);

                var reviews = _context.Reviews
                    .Include(r => r.Hotel)
                    .ThenInclude(h => h.Rooms.Where(r => r.Id == r.Id).Take(1))
                    .Where(r => r.CreatedAt >= startDate.Date && r.CreatedAt <= adjustedEndDate)
                    .OrderByDescending(r => r.CreatedAt)
                    .Select(r => new DashboardVM.UserReview
                    {
                        Content = r.Comment ?? "Không có đánh giá",
                        RoomType = r.Hotel.Rooms.FirstOrDefault().Type ?? "Không xác định"
                    })
                    .Take(20)
                    .ToList();

                if (reviews.Count == 0)
                {
                    // Nếu không có đánh giá trong khoảng thời gian, lấy đánh giá mới nhất
                    reviews = _context.Reviews
                        .Include(r => r.Hotel)
                        .ThenInclude(h => h.Rooms.Where(r => r.Id == r.Id).Take(1))
                        .OrderByDescending(r => r.CreatedAt)
                        .Select(r => new DashboardVM.UserReview
                        {
                            Content = r.Comment ?? "Không có đánh giá",
                            RoomType = r.Hotel.Rooms.FirstOrDefault().Type ?? "Không xác định"
                        })
                        .Take(20)
                        .ToList();
                }

                return reviews;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách đánh giá người dùng");
                return null;
            }
        }

        private string GetUserRole(string userId)
        {
            try
            {
                var userRole = _context.UserRoles
                    .Where(ur => ur.UserId == userId)
                    .Join(_context.Roles,
                         ur => ur.RoleId,
                         r => r.Id,
                         (ur, r) => r.Name)
                    .FirstOrDefault();

                return userRole ?? "Undefine";
            }
            catch
            {
                return "Undefine";
            }
        }
        public List<DashboardVM.StaffInfo> GetStaffInfos()
        {
            try
            {
                var users = _context.Users.AsNoTracking().ToList();
                var staffs = new List<DashboardVM.StaffInfo>();

                foreach (var user in users)
                {
                    string role = GetUserRole(user.Id);
                    if (role != "Customer" && role != "Undefine")
                    {
                        staffs.Add(new DashboardVM.StaffInfo
                        {
                            Id = user.Id,
                            Name = user.UserName ?? "Không có tên",
                            Role = role,
                            Status = user.EmailConfirmed ? "Xác thực" : "Chưa xác thực"
                        });
                    }
                }

                return staffs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách thông tin nhân viên");
                return new List<DashboardVM.StaffInfo>();
            }
        }

        public List<DashboardVM.PopularRoomType> GetPopularRoomTypes(DateTime startDate, DateTime endDate)
        {
            try
            {
                var adjustedEndDate = endDate.Date.AddDays(1).AddSeconds(-1);

                var successfulBookingIds = _context.Payments
                   .AsNoTracking()
                   .Where(p => p.Status == "Success")
                   .Select(p => p.BookingId)
                   .Distinct()
                   .ToList();

                var popularRoomTypes = _context.BookingsRoomDetails
                    .AsNoTracking()
                    .Where(brd => brd.Booking.CreatedAt >= startDate.Date && 
                                brd.Booking.CreatedAt <= adjustedEndDate &&
                                successfulBookingIds.Contains(brd.BookingId))
                    .GroupBy(brd => new { 
                                    RoomNumber = brd.Room.RoomNumber ?? "Unknown Room Type", 
                                    Image = brd.Room.Image ?? "~/images/room/room-default.png" })
                    .Select(g => new DashboardVM.PopularRoomType
                    {
                        RoomNumber = g.Key.RoomNumber,
                        ImageUrl = g.Key.Image,
                        BookingCount = g.Count(),
                        Revenue = g.Sum(brd => brd.Price ?? 0)
                    })
                    .OrderByDescending(r => r.BookingCount)
                    .Take(5)
                    .ToList();

                // Corrected condition: check if list is empty
                if (!popularRoomTypes.Any())
                {
                    _logger.LogWarning("No booking room data found for the date range {StartDate} to {EndDate}",
                        startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"));
                }

                return popularRoomTypes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách phòng phổ biến");
                return new List<DashboardVM.PopularRoomType>();
            }
        }

        #endregion

        #region Dashboard Data
        public async Task<DashboardVM> GetDashboardDataAsync(DateTime startDate, DateTime endDate)
        {
            var errors = new List<string>();
            var dashboard = new DashboardVM
            {
                StartDate = startDate,
                EndDate = endDate
            };

            try
            {
                // Bộ thống kê tổng quan - bắt các ngoại lệ riêng lẻ cho từng phần
                try
                {
                    int totalBookings = GetTotalBookings(startDate, endDate);
                    dashboard.TotalBookings = totalBookings;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Lỗi khi lấy dữ liệu tổng số đặt phòng");
                    errors.Add("Không thể tải dữ liệu đặt phòng");
                }

                try
                {
                    int totalCustomers = GetTotalCustomers();
                    dashboard.TotalCustomers = totalCustomers;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Lỗi khi lấy dữ liệu tổng số khách hàng");
                    errors.Add("Không thể tải dữ liệu khách hàng");
                }

                try
                {
                    decimal totalServiceRevenue = GetTotalServiceRevenue(startDate, endDate);
                    dashboard.TotalRevenueService = totalServiceRevenue;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Lỗi khi lấy dữ liệu tổng doanh thu dịch vụ");
                    errors.Add("Không thể tải dữ liệu doanh thu dịch vụ");
                }

                try
                {
                    decimal totalRoomRevenue = GetTotalRoomRevenue(startDate, endDate);
                    dashboard.TotalRevenueRoom = totalRoomRevenue;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Lỗi khi lấy dữ liệu tổng doanh thu phòng");
                    errors.Add("Không thể tải dữ liệu doanh thu phòng");
                }

                try
                {
                    dashboard.RevenueChartData = GetRevenueChartData(startDate, endDate);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Lỗi khi lấy dữ liệu biểu đồ doanh thu");
                    errors.Add("Không thể tải dữ liệu biểu đồ doanh thu");
                }

                try
                {
                    dashboard.BookingChartData = GetBookingChartData(startDate, endDate);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Lỗi khi lấy dữ liệu biểu đồ đặt phòng");
                    errors.Add("Không thể tải dữ liệu biểu đồ đặt phòng");
                }

                try
                {
                    dashboard.RoomTypeChartData = GetRoomTypeChartData(startDate, endDate);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Lỗi khi lấy dữ liệu biểu đồ loại phòng");
                    errors.Add("Không thể tải dữ liệu biểu đồ loại phòng");
                }

                try
                {
                    dashboard.ServiceRevenueItems = GetServiceRevenueItems(startDate, endDate);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Lỗi khi lấy danh sách doanh thu dịch vụ");
                    errors.Add("Không thể tải dữ liệu doanh thu dịch vụ");
                }

                try
                {
                    dashboard.RoomTypeRevenueItems = GetRoomTypeRevenueItems(startDate, endDate);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Lỗi khi lấy danh sách doanh thu loại phòng");
                    errors.Add("Không thể tải dữ liệu doanh thu loại phòng");
                }

                try
                {
                    dashboard.UserReviews = GetUserReviews(startDate, endDate);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Lỗi khi lấy danh sách đánh giá người dùng");
                    errors.Add("Không thể tải dữ liệu đánh giá người dùng");
                }

                try
                {
                    dashboard.StaffInfos = GetStaffInfos();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Lỗi khi lấy danh sách thông tin nhân viên");
                    errors.Add("Không thể tải dữ liệu thông tin nhân viên");
                }

                try
                {
                    dashboard.PopularRoomTypes = GetPopularRoomTypes(startDate, endDate);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Lỗi khi lấy danh sách phòng phổ biến");
                    errors.Add("Không thể tải dữ liệu phòng phổ biến");
                }

                // Nếu có lỗi, lưu vào ViewData để hiển thị
                if (errors.Any())
                {
                    throw new DashboardServiceException("Có lỗi khi tải dữ liệu dashboard", null)
                    {
                        Errors = errors
                    };
                }

                return dashboard;
            }
            catch (DashboardServiceException ex)
            {
                // Đã có lỗi được xử lý, ném lại để controller xử lý
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi không xác định khi tải dữ liệu dashboard");
                errors.Add("Có lỗi không xác định khi tải dữ liệu dashboard");

                throw new DashboardServiceException("Có lỗi không xác định khi tải dữ liệu dashboard", ex)
                {
                    Errors = errors
                };
            }
        }
        #endregion

        #region Export Excel
        public List<ServiceRevenueExcel> GetServiceRevenueExcelData(DateTime startDate, DateTime endDate)
        {
            try
            {
                var adjustedEndDate = endDate.Date.AddDays(1).AddSeconds(-1);

                var serviceRevenues = _context.BookingsServiceDetails
                    .AsNoTracking()
                    .Where(bsd => bsd.Booking.CreatedAt >= startDate.Date &&
                                bsd.Booking.CreatedAt <= adjustedEndDate)
                    .Include(bsd => bsd.Service)
                    .Include(bsd => bsd.Booking)
                        .ThenInclude(b => b.User)
                    .Include(bsd => bsd.Booking)
                        .ThenInclude(b => b.Staff)
                    .Include(bsd => bsd.Booking)
                        .ThenInclude(b => b.Payments)
                    .Select(bsd => new ServiceRevenueExcel
                    {
                        Id = bsd.Id,
                        ServiceId = bsd.ServiceId,
                        BookingId = bsd.BookingId,
                        ServiceName = bsd.Service.Name,
                        Price = bsd.Price ?? 0,
                        CustomerName = bsd.Booking.User.UserName,
                        StaffName = bsd.Booking.Staff.UserName,
                        CreatedAt = bsd.Booking.CreatedAt ?? DateTime.MinValue,
                        PaymentStatus = bsd.Booking.Payments
                                        .Where(p => p.BookingId == bsd.BookingId)
                                        .OrderByDescending(p => p.PaymentDate)
                                        .Select(p => p.Status)
                                        .FirstOrDefault() ?? "Pending",
                        PaymentDate = bsd.Booking.Payments
                                        .Where(p => p.BookingId == bsd.BookingId)
                                        .OrderByDescending(p => p.PaymentDate)
                                        .Select(p => p.PaymentDate)
                                        .FirstOrDefault()
                    })
                    .OrderBy(srv => srv.CreatedAt)
                    .ToList();

                return serviceRevenues;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách doanh thu dịch vụ để xuất ra Excel");
                return new List<ServiceRevenueExcel>();
            }
        }

        public List<RoomRevenueExcel> GetRoomRevenueExcelData(DateTime startDate, DateTime endDate)
        {
            try
            {
                var adjustedEndDate = endDate.Date.AddDays(1).AddSeconds(-1);

                var roomRevenues = _context.BookingsRoomDetails
                    .AsNoTracking()
                    .Where(brd => brd.Booking.CreatedAt >= startDate.Date &&
                                brd.Booking.CreatedAt <= adjustedEndDate)
                    .Include(brd => brd.Room)
                    .Include(brd => brd.Booking)
                        .ThenInclude(b => b.User)
                    .Include(brd => brd.Booking)
                        .ThenInclude(b => b.Staff)
                    .Include(brd => brd.Booking)
                        .ThenInclude(b => b.Payments)
                    .Select(brd => new RoomRevenueExcel
                    {
                        Id = brd.Id,
                        RoomId = brd.RoomId,
                        BookingId = brd.BookingId,
                        RoomNumber = brd.Room.RoomNumber,
                        RoomType = brd.Room.Type,
                        Price = brd.Price ?? 0,
                        CustomerName = brd.Booking.User.UserName,
                        StaffName = brd.Booking.Staff.UserName,
                        CheckIn = brd.CheckIn ?? DateOnly.MinValue,
                        CheckOut = brd.CheckOut ?? DateOnly.MinValue,
                        CreatedAt = brd.Booking.CreatedAt ?? DateTime.MinValue,
                        PaymentStatus = brd.Booking.Payments
                                        .Where(p => p.BookingId == brd.BookingId)
                                        .OrderByDescending(p => p.PaymentDate)
                                        .Select(p => p.Status)
                                        .FirstOrDefault() ?? "Pending",
                        PaymentDate = brd.Booking.Payments
                                        .Where(p => p.BookingId == brd.BookingId)
                                        .OrderByDescending(p => p.PaymentDate)
                                        .Select(p => p.PaymentDate)
                                        .FirstOrDefault()
                    })
                    .OrderBy(rtr => rtr.CreatedAt)
                    .ToList();

                return roomRevenues;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách doanh thu loại phòng để xuất ra Excel");
                return new List<RoomRevenueExcel>();
            }
        }

        public List<RevenueExcel> GetRevenueExcelData(DateTime startDate, DateTime endDate)
        {
            try
            {
                var adjustedEndDate = endDate.Date.AddDays(1).AddSeconds(-1);
                var result = new List<RevenueExcel>();

                // First approach: Get bookings with both room and service information
                var bookings = _context.Bookings
                    .AsNoTracking()
                    .Where(b => b.CreatedAt >= startDate.Date &&
                               b.CreatedAt <= adjustedEndDate)
                    .Include(b => b.User)
                    .Include(b => b.Staff)
                    .Include(b => b.Payments)
                    .Include(b => b.BookingsRoomDetails)
                        .ThenInclude(brd => brd.Room)
                    .Include(b => b.BookingsServiceDetails)
                        .ThenInclude(bsd => bsd.Service)
                    .ToList();

                foreach (var booking in bookings)
                {
                    // Get payment info
                    var payment = booking.Payments
                        .OrderByDescending(p => p.PaymentDate)
                        .FirstOrDefault();

                    // Calculate service prices for this booking
                    decimal servicePrice = booking.BookingsServiceDetails
                        .Sum(bsd => bsd.Price ?? 0);

                    // Get service names (combined with comma if multiple)
                    string serviceNames = string.Join(", ",
                        booking.BookingsServiceDetails
                            .Select(bsd => bsd.Service?.Name)
                            .Where(name => !string.IsNullOrEmpty(name))
                            .Distinct()
                            .ToArray());

                    // Calculate room prices for this booking
                    decimal roomPrice = booking.BookingsRoomDetails
                        .Sum(brd => brd.Price ?? 0);

                    // Get room types (combined with comma if multiple)
                    string roomTypes = string.Join(", ",
                        booking.BookingsRoomDetails
                            .Select(brd => brd.Room?.Type)
                            .Where(type => !string.IsNullOrEmpty(type))
                            .Distinct()
                            .ToArray());

                    // Get check-in and check-out dates (use first room's dates or default)
                    var firstRoom = booking.BookingsRoomDetails.FirstOrDefault();
                    DateOnly checkIn = firstRoom?.CheckIn ?? DateOnly.MinValue;
                    DateOnly checkOut = firstRoom?.CheckOut ?? DateOnly.MinValue;

                    // Create combined revenue item
                    var revenueItem = new RevenueExcel
                    {
                        BookingId = booking.Id,
                        ServiceName = serviceNames,
                        ServicePrice = servicePrice,
                        RoomType = roomTypes,
                        RoomPrice = roomPrice,
                        TotalPrice = servicePrice + roomPrice,
                        CustomerName = booking.User?.UserName,
                        StaffName = booking.Staff?.UserName,
                        CheckIn = checkIn,
                        CheckOut = checkOut,
                        CreatedAt = booking.CreatedAt ?? DateTime.MinValue,
                        PaymentStatus = payment?.Status ?? "Pending",
                        PaymentDate = payment?.PaymentDate
                    };

                    result.Add(revenueItem);
                }

                return result.OrderBy(r => r.CreatedAt).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy dữ liệu doanh thu tổng hợp để xuất ra Excel");
                return new List<RevenueExcel>();
            }
        }

        #endregion

    }

    /// <summary>
    /// Lớp ngoại lệ riêng cho DashboardServices
    /// </summary>
    public class DashboardServiceException : Exception
    {
        public List<string> Errors { get; set; } = new List<string>();

        public DashboardServiceException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
