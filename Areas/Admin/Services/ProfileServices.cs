using Hotel_Management.Areas.Admin.Services.Interfaces;
using Hotel_Management.Areas.Admin.ViewModels;
using Hotel_Management.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Hotel_Management.Areas.Admin.Services
{
    public class ProfileServices : IProfileServices
    {
        private readonly HotelManagementContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileServices(HotelManagementContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public ProfileVM GetProfile(ClaimsPrincipal user)
        {
            var appUser = _userManager.GetUserAsync(user).Result;
            if (appUser == null) return null!;

            return new ProfileVM
            {
                UserName = appUser.UserName ?? "",
                Email = appUser.Email ?? "",
                PhoneNumber = appUser.PhoneNumber,
                // Không trả về mật khẩu
                Password = "",
                ConfirmPassword = "",
                PerformanceChartData = new List<ProfileVM.ChartDataPoint>()
            };
        }

        public bool UpdateProfile(ProfileVM model, ClaimsPrincipal user)
        {
            var appUser = _userManager.GetUserAsync(user).Result;
            if (appUser == null) return false;

            appUser.UserName = model.UserName;
            appUser.Email = model.Email;
            appUser.PhoneNumber = model.PhoneNumber;

            // Nếu muốn đổi mật khẩu
            if (!string.IsNullOrEmpty(model.Password))
            {
                var token = _userManager.GeneratePasswordResetTokenAsync(appUser).Result;
                var result = _userManager.ResetPasswordAsync(appUser, token, model.Password).Result;
                if (!result.Succeeded) return false;
            }

            var updateResult = _userManager.UpdateAsync(appUser).Result;
            return updateResult.Succeeded;
        }

        public List<ProfileVM.ChartDataPoint> GetPerformanceChartData(ClaimsPrincipal user, DateTime startDate, DateTime endDate)
        {
            var appUser = _userManager.GetUserAsync(user).Result;
            if (appUser == null) return new List<ProfileVM.ChartDataPoint>();

            var adjustedEndDate = endDate.Date.AddDays(1).AddSeconds(-1);

            // Lấy số lượng booking thành công theo từng ngày
            var bookingByDay = _context.Bookings
                .AsNoTracking()
                .Where(b => b.StaffId == appUser.Id
                            && b.CreatedAt.HasValue
                            && b.CreatedAt.Value >= startDate.Date
                            && b.CreatedAt.Value <= adjustedEndDate
                            && b.Payments.Any(p => p.Status == "Success"))
                .GroupBy(b => b.CreatedAt.Value.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    Count = g.Count()
                })
                .ToDictionary(x => x.Date, x => x.Count);

            // Tạo danh sách đủ các ngày trong khoảng, kể cả ngày không có booking
            var result = new List<ProfileVM.ChartDataPoint>();
            for (var date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
            {
                int count = bookingByDay.TryGetValue(date, out var c) ? c : 0;
                result.Add(new ProfileVM.ChartDataPoint
                {
                    Label = date.ToString("dd/MM"),
                    Value = count
                });
            }

            return result;
        }
    }
}
