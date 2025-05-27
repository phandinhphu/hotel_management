using Hotel_Management.Helpers;
using Hotel_Management.Models;

namespace Hotel_Management.Areas.Admin.Services.Interfaces
{
    public interface IStaffServices
    {
        Task<PaginatedList<ApplicationUser>> getAllAsync(string name = "", int pageIndex = 1, int pageSize = 20);
        Task<ApplicationUser> getByIdAsync(string id);
    }
}
