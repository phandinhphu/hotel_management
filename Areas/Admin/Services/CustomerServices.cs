using Hotel_Management.Areas.Admin.Services.Interfaces;
using Hotel_Management.Helpers;
using Hotel_Management.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Hotel_Management.Areas.Admin.Services
{
    public class CustomerServices : ICustomerServices
    {
        private readonly HotelManagementContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CustomerServices(HotelManagementContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<PaginatedList<ApplicationUser>> getAllAsync(string name = "", int pageIndex = 1, int pageSize = 20)
        {
            var user = await _userManager.GetUsersInRoleAsync("Customer");

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

        public async Task<DateOnly?> getLastCheckInByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id), "Id cannot be null or empty.");
            }

            var lastCheckIn = await _context.BookingsRoomDetails
                            .Where(d => d.Booking.UserId == id)
                            .OrderByDescending(d => d.CheckIn)
                            .Select(d => d.CheckIn)
                            .FirstOrDefaultAsync();


            if (lastCheckIn == default)
            {
                return await Task.FromResult<DateOnly?>(DateOnly.MinValue);
            }

            return await Task.FromResult<DateOnly?>(lastCheckIn);
        }

        public async Task<decimal?> getTotalSpentByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id), "Id cannot be null or empty.");
            }

            var totalSpent = await _context.Bookings
                .Where(b => b.UserId == id)
                .SumAsync(b => b.TotalPrice);

            if (totalSpent < 0)
            {
                throw new InvalidOperationException($"No bookings found for user with id '{id}'.");
            }
            return await Task.FromResult<decimal?>(totalSpent > 0 ? totalSpent : null);
        }
    }
}
