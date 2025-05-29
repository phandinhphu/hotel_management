using Hotel_Management.Models;

namespace Hotel_Management.Services.Interfaces
{
    public interface IHotelservicesService
    {
        Task<IEnumerable<Service>> GetAllHotelServicesAsync();
        Task<Service> GetHotelServiceByIdAsync(int id);
    }
}
