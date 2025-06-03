using Hotel_Management.Areas.Admin.Services.Interfaces;
using Hotel_Management.Models;
using Microsoft.EntityFrameworkCore;

namespace Hotel_Management.Areas.Admin.Services
{
    public class HotelServices : IHotelServices
    {
        private readonly HotelManagementContext _context;

        public HotelServices(HotelManagementContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Hotel>> GetAllAsync()
        {
            return await _context.Hotels.ToListAsync();
        }

        public async Task<Hotel> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentNullException(nameof(id), "Hotel ID must be greater than zero.");
            }
            var hotel = await _context.Hotels.FindAsync(id);
            if (hotel == null)
            {
                throw new InvalidOperationException($"Hotel with ID {id} not found.");
            }
            return hotel;
        }
    }
}
