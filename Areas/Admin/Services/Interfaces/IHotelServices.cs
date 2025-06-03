using Hotel_Management.Models;

namespace Hotel_Management.Areas.Admin.Services.Interfaces
{
    public interface IHotelServices
    {
        Task<IEnumerable<Hotel>> GetAllAsync();
        Task<Hotel> GetByIdAsync(int id);
    }
}
