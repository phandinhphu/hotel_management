using Hotel_Management.Models;

namespace Hotel_Management.Services.Interfaces
{
    public interface IHotelservicesService
    {
        Task<IEnumerable<Service>> GetAllHotelServicesAsync(string serviceName = "", int pageIndex = 1, int pageSize = 20);
        Task<Service> GetHotelServiceByIdAsync(int id);
    }
}
