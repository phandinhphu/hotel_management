using Hotel_Management.Models;
using Hotel_Management.Services.Interfaces;

namespace Hotel_Management.Services
{
    public class HotelservicesService : IHotelservicesService
    {
        private readonly HotelManagementContext _context;
        public IEnumerable<Service> MorkServices { get; set; } = new List<Service>()
        {
            new Service
            {
                Id = 1,
                HotelId = 1,
                Name = "Room Service",
                Description = "24/7 room service for food and beverages.",
                Price = 20.00m
            },
            new Service
            {
                Id = 2,
                HotelId = 1,
                Name = "Laundry Service",
                Description = "Same-day laundry service for your convenience.",
                Price = 15.00m
            },
            new Service
            {
                Id = 3,
                HotelId = 1,
                Name = "Spa and Wellness",
                Description = "Relaxing spa treatments and wellness packages.",
                Price = 50.00m
            },
            new Service
            {
                Id = 4,
                HotelId = 1,
                Name = "Airport Shuttle",
                Description = "Complimentary airport shuttle service.",
                Price = 0.00m
            }
        };

        public HotelservicesService(HotelManagementContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Service>> GetAllHotelServicesAsync(string serviceName = "", int pageIndex = 1, int pageSize = 20)
        {
            return await Task.FromResult(MorkServices);
        }

        public async Task<Service> GetHotelServiceByIdAsync(int id)
        {
            return await Task.FromResult(MorkServices.FirstOrDefault(s => s.Id == id) ?? new Service());
        }
    }
}
