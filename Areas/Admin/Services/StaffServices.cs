using Hotel_Management.Areas.Admin.Services.Interfaces;
using Hotel_Management.Helpers;
using Hotel_Management.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Hotel_Management.Areas.Admin.Services
{
    public class StaffServices : IUserServices, IStaffServices
    {
        private readonly HotelManagementContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public StaffServices(
            HotelManagementContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<PaginatedList<ApplicationUser>> getAllAsync(string name = "", int pageIndex = 1, int pageSize = 20)
        {
            var user = await _userManager.GetUsersInRoleAsync("Staff");

            if (!string.IsNullOrEmpty(name))
            {
                user = user.Where(u => u.UserName.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return await PaginatedList<ApplicationUser>.Create(user, pageIndex, pageSize);
        }

        public async Task<ApplicationUser> getByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id), "Id cannot be null or empty.");
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                throw new InvalidOperationException($"User with id '{id}' not found.");
            }

            return user;
        }

        public async Task<int> getTotalBookedRoomsByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id), "Id cannot be null or empty.");
            }

            var user = await _context.Users
                .Include(u => u.BookingStaffs)
                    .ThenInclude(b => b.BookingsRoomDetails)
                    .ThenInclude(brd => brd.Room)
                .FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                throw new InvalidOperationException($"User with id '{id}' not found.");
            }

            return user.BookingStaffs.Sum(b => b.BookingsRoomDetails.Count);
        }

        public async Task<bool> ResetPasswordAsync(string userId, string newPassword)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId), "User ID cannot be null or empty.");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new InvalidOperationException($"User with id '{userId}' not found.");
            }

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);

            if (result.Succeeded)
            {
                return true;
            }
            else
            {
                throw new InvalidOperationException($"Failed to reset password for user with id '{userId}': {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
        }
    }
}
