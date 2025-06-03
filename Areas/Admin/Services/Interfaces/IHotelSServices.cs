using Hotel_Management.Helpers;
using Hotel_Management.Models;

namespace Hotel_Management.Areas.Admin.Services.Interfaces
{
    public interface IHotelSServices
    {
        Task<PaginatedList<Service>> GetAllAsync(string? querySearch, int pageIndex, int pageSize = 10);
        Task<Service?> GetByIdAsync(int id);
        Task<bool> CreateAsync(Service service);
        Task<bool> UpdateAsync(Service service);
        Task<bool> DeleteAsync(int id);
    }
}
