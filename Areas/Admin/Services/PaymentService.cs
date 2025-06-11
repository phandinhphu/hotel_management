using Microsoft.EntityFrameworkCore;
using Hotel_Management.Helpers;
using Hotel_Management.Models;
using Hotel_Management.Areas.Admin.ViewModels;
using Hotel_Management.Areas.Admin.Services.Interfaces;

namespace Hotel_Management.Areas.Admin.Services
{
    public enum PaymentStatus
    {
        Paid,
        Pending,
        Failed
    }
    public class PaymentService : IPaymentService
    {
        private readonly HotelManagementContext _context;
        public PaymentService(HotelManagementContext context)
        {
            _context = context;
        }

        public async Task<PaginatedList<PaymentVM>> GetAllAsync(string searchTerm = "", int pageIndex = 1, int pageSize = 20)
        {
            var query = _context.Payments
                .AsNoTracking()
                .Include(p => p.Booking)
                    .ThenInclude(b => b.User)
                .Include(p => p.Booking)
                    .ThenInclude(b => b.BookingsRoomDetails)
                        .ThenInclude(brd => brd.Room)
                .Include(p => p.Booking)
                    .ThenInclude(b => b.Staff)
                .AsQueryable();

            // Search
            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.Trim().ToLower();
                query = query.Where(p => p.Booking.User.UserName.ToLower().Contains(searchTerm) ||
                                         p.Booking.User.Email.ToLower().Contains(searchTerm) ||
                                         p.Booking.User.PhoneNumber.ToLower().Contains(searchTerm) ||
                                         p.Booking.Id.ToString().Contains(searchTerm)
                );
            }

            query = query.OrderByDescending(p => p.PaymentDate);

            var mappedQuery = query.Select(p => new PaymentVM
            {
                Id = p.Id,
                BookingId = p.BookingId ?? 0,
                CustomerName = p.Booking.User.UserName ?? "",
                PhoneNumber = p.Booking.User.PhoneNumber ?? "",
                Email = p.Booking.User.Email ?? "",
                CheckIn = p.Booking.BookingsRoomDetails
                    .OrderBy(brd => brd.CheckIn)
                    .Select(brd => brd.CheckIn.HasValue
                        ? new DateTime(brd.CheckIn.Value.Year, brd.CheckIn.Value.Month, brd.CheckIn.Value.Day)
                        : DateTime.MinValue)
                    .FirstOrDefault(),
                CheckOut = p.Booking.BookingsRoomDetails
                    .OrderByDescending(brd => brd.CheckOut)
                    .Select(brd => brd.CheckOut.HasValue
                        ? new DateTime(brd.CheckOut.Value.Year, brd.CheckOut.Value.Month, brd.CheckOut.Value.Day)
                        : DateTime.MinValue)
                    .FirstOrDefault(),
                PaymentDate = p.PaymentDate ?? DateTime.MinValue,
                RoomPrice = p.Booking.TotalPriceRooms ?? 0,
                ServicePrice = p.Booking.TotalPriceServices ?? 0,
                Amount = p.Amount ?? 0,
                PaymentMethod = p.PaymentMethod ?? "",
                Status = p.Status ?? "",
                Items = p.Booking.BookingsRoomDetails
                    .Select(brd => new PaymentItemVM
                    {
                        Name = $"{brd.Room.Type ?? "Room"} ({brd.Room.RoomNumber})",
                        Price = brd.Price ?? 0
                    }).ToList()
            });

            return await PaginatedList<PaymentVM>.Create(mappedQuery, pageIndex, pageSize);
        }

        public async Task<PaymentVM> GetByIdAsync(int id)
        {
            var payment = await _context.Payments
                .AsNoTracking()
                .Include(p => p.Booking)
                    .ThenInclude(b => b.User)
                .Include(p => p.Booking)
                    .ThenInclude(b => b.BookingsRoomDetails)
                        .ThenInclude(brd => brd.Room)
                .Include(p => p.Booking)
                    .ThenInclude(b => b.Staff)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (payment == null)
            {
                return null;
            }
            return new PaymentVM
            {
                Id = payment.Id,
                BookingId = payment.BookingId ?? 0,
                CustomerName = payment.Booking.User.UserName ?? "",
                StaffName = payment.Booking.Staff?.UserName ?? "",
                PhoneNumber = payment.Booking.User.PhoneNumber ?? "",
                Email = payment.Booking.User.Email ?? "",
                CheckIn = payment.Booking.BookingsRoomDetails
                    .OrderBy(brd => brd.CheckIn)
                    .Select(brd => brd.CheckIn.HasValue
                        ? new DateTime(brd.CheckIn.Value.Year, brd.CheckIn.Value.Month, brd.CheckIn.Value.Day)
                        : DateTime.MinValue)
                    .FirstOrDefault(),
                CheckOut = payment.Booking.BookingsRoomDetails
                    .OrderByDescending(brd => brd.CheckOut)
                    .Select(brd => brd.CheckOut.HasValue
                        ? new DateTime(brd.CheckOut.Value.Year, brd.CheckOut.Value.Month, brd.CheckOut.Value.Day)
                        : DateTime.MinValue)
                    .FirstOrDefault(),
                PaymentDate = payment.PaymentDate ?? DateTime.MinValue,
                RoomPrice = payment.Booking.TotalPriceRooms ?? 0,
                ServicePrice = payment.Booking.TotalPriceServices ?? 0,
                Amount = payment.Amount ?? 0,
                PaymentMethod = payment.PaymentMethod ?? "",
                Status = payment.Status ?? "",
                Items = payment.Booking.BookingsRoomDetails
                    .Select(brd => new PaymentItemVM
                    {
                        Name = $"Phòng {brd.Room.Type ?? "Room"} ({brd.Room.RoomNumber})",
                        Price = brd.Price ?? 0
                    }).ToList()
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var payment = await _context.Payments
                .Include(p => p.Booking)
                    .ThenInclude(b => b.BookingsRoomDetails)
                .Include(p => p.Booking)
                    .ThenInclude(b => b.BookingsServiceDetails)
                .FirstOrDefaultAsync(p => p.Id == id);

                if (payment == null)
                {
                    return false;
                }

                // Kiểm tra xem đây có phải là thanh toán duy nhất cho booking không
                bool isOnlyPayment = await _context.Payments
                    .CountAsync(p => p.BookingId == payment.BookingId) == 1;

                if (isOnlyPayment && payment.Booking != null)
                {
                    // Delete BookingsRoomDetails
                    if (payment.Booking.BookingsRoomDetails != null && payment.Booking.BookingsRoomDetails.Any())
                    {
                        _context.BookingsRoomDetails.RemoveRange(payment.Booking.BookingsRoomDetails);
                    }

                    // Delete Booking Services
                    if (payment.Booking.BookingsServiceDetails != null && payment.Booking.BookingsServiceDetails.Any())
                    {
                        _context.BookingsServiceDetails.RemoveRange(payment.Booking.BookingsServiceDetails);
                    }

                    // Delete Booking
                    _context.Bookings.Remove(payment.Booking);
                }

                // Delete Payment
                _context.Payments.Remove(payment);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine($"Error deleting payment: {ex.Message}");
                return false;
            }
        }
    }
}
