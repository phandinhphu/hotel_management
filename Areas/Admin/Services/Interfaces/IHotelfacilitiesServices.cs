using Hotel_Management.Helpers;
using Hotel_Management.Models;

namespace Hotel_Management.Areas.Admin.Services.Interfaces
{
    public interface IHotelfacilitiesServices
    {
        Task<PaginatedList<Hotelfacility>> GetAllAsync(int pageIndex = 1, int pageSize = 20);
        Task<Hotelfacility> GetByIdAsync(int id);
        Task<bool> CreateAsync(Hotelfacility hotelfacility);
        Task<bool> UpdateAsync(Hotelfacility hotelfacility);
        Task<bool> DeleteAsync(int id);
    }
}
