using Hotel_Management.Helpers;
using Hotel_Management.Models;
using Hotel_Management.Areas.Admin.ViewModels;
using Hotel_Management.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Hotel_Management.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<PaginatedList<UserAndRoleVM>> GetUsersWithRoleAsync(string selectedRole = "", string searchString = "", int pageIndex = 1, int pageSize = 20)
        {
            var users = _userManager.Users.ToList();
            var result = new List<UserAndRoleVM>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var role = roles.FirstOrDefault() ?? "No role";

                result.Add(new UserAndRoleVM
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Role = role
                });
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                result = result
                    .Where(u => u.UserName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            if (!string.IsNullOrEmpty(selectedRole))
            {
                result = result
                    .Where(u => u.Role.Equals(selectedRole, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            return await PaginatedList<UserAndRoleVM>.Create(result, pageIndex, pageSize);
        }
    }
}
