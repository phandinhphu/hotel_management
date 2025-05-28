using Hotel_Management.Areas.Admin.ViewModels;
using Hotel_Management.Helpers;
using Hotel_Management.Models;

namespace Hotel_Management.Services.Interfaces
{
    public interface IUserService
    {
        Task<PaginatedList<UserAndRoleVM>> GetUsersWithRoleAsync(string role = "", string searchString = "", int pageIndex = 1, int pageSize = 20);
    }
}
