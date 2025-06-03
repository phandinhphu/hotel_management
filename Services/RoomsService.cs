using Hotel_Management.Helpers;
using Hotel_Management.Models;
using Hotel_Management.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hotel_Management.Services
{
    public class RoomsService : IRoomsService
    {
        private readonly HotelManagementContext _context;

        public RoomsService(HotelManagementContext context)
        {
            _context = context;
        }

        public async Task<PaginatedList<Room>> GetAllRoomsAsync(string status = "", int pageIndex = 1, int pageSize = 20)
        {
            var query = _context.Rooms.AsQueryable();

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(r => r.Status != null && r.Status.ToLower().Contains(status.ToLower()));
            }

            return await PaginatedList<Room>.Create(
                query.Include(r => r.Roomimages),
                pageIndex,
                pageSize
            );
        }

        public async Task<Room> GetRoomByIdAsync(int id)
        {
            var room = await _context.Rooms
                .Include(r => r.Roomimages)
                .Where(r => r.Id == id)
                .FirstOrDefaultAsync();

            return await Task.FromResult(room);
        }
    }
}
