using Hotel_Management.Models;
using Hotel_Management.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hotel_Management.Services
{
    public class HotelservicesService : IHotelservicesService
    {
        private readonly HotelManagementContext _context;

        public HotelservicesService(HotelManagementContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Service>> GetAllHotelServicesAsync()
        {
            var services = _context.Services
                .Where(s => s.HotelId == 1)
                .ToList();

            return await Task.FromResult(services);
        }

        public async Task<Service> GetHotelServiceByIdAsync(int id)
        {
            var service = await _context.Services
                .FirstOrDefaultAsync(s => s.Id == id && s.HotelId == 1);

            if (service == null)
            {
                throw new KeyNotFoundException($"Service with ID {id} not found.");
            }

            return service;
        }
    }
}
