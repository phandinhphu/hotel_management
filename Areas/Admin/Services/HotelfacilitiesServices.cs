using Hotel_Management.Areas.Admin.Services.Interfaces;
using Hotel_Management.Helpers;
using Hotel_Management.Models;
using Microsoft.EntityFrameworkCore;

namespace Hotel_Management.Areas.Admin.Services
{
    public class HotelfacilitiesServices : IHotelfacilitiesServices
    {
        private readonly HotelManagementContext _context;
        private readonly ILogger<HotelfacilitiesServices> _logger;

        public HotelfacilitiesServices(HotelManagementContext context, ILogger<HotelfacilitiesServices> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> CreateAsync(Hotelfacility hotelfacility)
        {
            if (hotelfacility == null)
            {
                throw new ArgumentNullException(nameof(hotelfacility));
            }

            try
            {
                await _context.Hotelfacilities.AddAsync(hotelfacility);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating hotelfacility");
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var hotelfacility = await _context.Hotelfacilities.FindAsync(id);
            if (hotelfacility == null)
            {
                return false; // Not found
            }

            try
            {
                _context.Hotelfacilities.Remove(hotelfacility);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting hotelfacility with ID {Id}", id);
                return false;
            }
        }

        public async Task<PaginatedList<Hotelfacility>> GetAllAsync(int pageIndex = 1, int pageSize = 20)
        {
            try
            {
                var query = _context.Hotelfacilities
                            .Include(h => h.Hotel)
                            .AsQueryable();

                return await PaginatedList<Hotelfacility>.Create(query, pageIndex, pageSize);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving hotelfacilities");
                return new PaginatedList<Hotelfacility>(new List<Hotelfacility>(), 0, pageIndex, pageSize);
            }
        }

        public async Task<Hotelfacility> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "Id must be greater than zero.");
            }

            var hotelfacility = await _context.Hotelfacilities
                .Include(h => h.Hotel)
                .FirstOrDefaultAsync(h => h.Id == id);

            if (hotelfacility == null)
            {
                throw new KeyNotFoundException($"Hotelfacility with ID {id} not found.");
            }
            return hotelfacility;
        }

        public async Task<bool> UpdateAsync(Hotelfacility hotelfacility)
        {
            if (hotelfacility == null)
            {
                throw new ArgumentNullException(nameof(hotelfacility));
            }

            try
            {
                _context.Hotelfacilities.Update(hotelfacility);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency error updating hotelfacility with ID {Id}", hotelfacility.Id);
                return false; // Handle concurrency issues
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating hotelfacility with ID {Id}", hotelfacility.Id);
                return false;
            }
        }
    }
}
