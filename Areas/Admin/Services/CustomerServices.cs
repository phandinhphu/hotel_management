using System;
using Hotel_Management.Areas.Admin.Services.Interfaces;
using Hotel_Management.Helpers;
using Hotel_Management.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Hotel_Management.Areas.Admin.Services
{
    public class CustomerServices : ICustomerServices, IUserServices
    {
        private readonly HotelManagementContext _context;
        private readonly IDbContextFactory<HotelManagementContext> _contextFactory;
        private readonly UserManager<ApplicationUser> _userManager;

        public CustomerServices(
            HotelManagementContext context,
            UserManager<ApplicationUser> userManager,
            IDbContextFactory<HotelManagementContext> dbContextFactory)
        {
            _context = context;
            _userManager = userManager;
            _contextFactory = dbContextFactory;
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

            await using var context = _contextFactory.CreateDbContext();

            var lastCheckIn = await context.BookingsRoomDetails
                            .Where(d => d.Booking.UserId == id)
                            .OrderByDescending(d => d.CheckIn)
                            .Select(d => d.CheckIn)
                            .FirstOrDefaultAsync();


            if (lastCheckIn == default)
            {
                return await Task.FromResult<DateOnly?>(DateOnly.MinValue);
            }

            return lastCheckIn;
        }

        public async Task<decimal?> getTotalSpentByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id), "Id cannot be null or empty.");
            }

            await using var context = _contextFactory.CreateDbContext();

            var totalSpent = await context.Bookings
                .Where(b => b.UserId == id)
                .SumAsync(b => b.TotalPrice);

            if (totalSpent < 0)
            {
                throw new InvalidOperationException($"No bookings found for user with id '{id}'.");
            }
            return totalSpent > 0 ? totalSpent : null;
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
