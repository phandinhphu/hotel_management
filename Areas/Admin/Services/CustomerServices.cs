using Hotel_Management.Areas.Admin.Services.Interfaces;
using Hotel_Management.Helpers;
using Hotel_Management.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Hotel_Management.Areas.Admin.Services
{
    public class StaffServices : IStaffServices
    {
        private readonly HotelManagementContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public StaffServices(HotelManagementContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Task<PaginatedList<ApplicationUser>> getAllAsync(string name = "", int pageIndex = 1, int pageSize = 20)
        {
            var user = _userManager.GetUsersInRoleAsync("Staff").Result;

            if (!string.IsNullOrEmpty(name))
            {
                user = user.Where(u => u.UserName.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return PaginatedList<ApplicationUser>.Create(
                user,
                pageIndex,
                pageSize
            );
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
    }
}
