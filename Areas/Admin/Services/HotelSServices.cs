using Hotel_Management.Areas.Admin.Services.Interfaces;
using Hotel_Management.Helpers;
using Hotel_Management.Models;
using Microsoft.EntityFrameworkCore;

namespace Hotel_Management.Areas.Admin.Services
{
    public class HotelSServices : IHotelSServices
    {
        private readonly HotelManagementContext _context;

        public HotelSServices(HotelManagementContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateAsync(Service service)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service), "Service cannot be null.");
            }
            _context.Services.Add(service);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentNullException(nameof(id), "Service ID must be greater than zero.");
            }
            var service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                throw new InvalidOperationException($"Service with ID {id} not found.");
            }
            _context.Services.Remove(service);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<PaginatedList<Service>> GetAllAsync(string? querySearch, int pageIndex, int pageSize = 10)
        {
            var query = _context.Services
                        .Include(s => s.Hotel)
                        .AsQueryable();

            if (!string.IsNullOrEmpty(querySearch))
            {
                query = query.Where(s => s.Name.Contains(querySearch));
            }

            return await PaginatedList<Service>.Create(
                query.OrderBy(s => s.Name),
                pageIndex,
                pageSize);
        }

        public async Task<Service?> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentNullException(nameof(id), "Service ID must be greater than zero.");
            }

            var service = await _context.Services
                        .Include(s => s.Hotel)
                        .FirstOrDefaultAsync(s => s.Id == id);

            if (service == null)
            {
                throw new InvalidOperationException($"Service with ID {id} not found.");
            }
            return service;
        }

        public async Task<bool> UpdateAsync(Service service)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service), "Service cannot be null.");
            }
            _context.Services.Update(service);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
