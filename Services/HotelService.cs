using Hotel_Management.Models;
using Hotel_Management.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hotel_Management.Services
{
    public class HotelService : IHotelService
    {
        private readonly HotelManagementContext _context;

        public HotelService(HotelManagementContext context)
        {
            _context = context;
        }

        public async Task<Hotel> getInfoHotel()
        {
            return await _context.Hotels.FirstOrDefaultAsync();
        }
    }
}
