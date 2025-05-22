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

        public async Task<PaginatedList<Room>> GetAllRoomsAsync(string roomNumber = "", int pageIndex = 1, int pageSize = 20)
        {
            var query = _context.Rooms.AsQueryable();

            if (!string.IsNullOrEmpty(roomNumber))
            {
                query = query.Where(r => r.RoomNumber != null && r.RoomNumber.Contains(roomNumber));
            }

            return await PaginatedList<Room>.Create(
                query.Include(r => r.Roomimages),
                pageIndex, 
                pageSize
            );
        }

        public async Task<Room> GetRoomsByIdAsync(int id)
        {
            var room = await _context.Rooms
                .Include(r => r.Roomimages)
                .Where(r => r.HotelId == id)
                .FirstOrDefaultAsync();

            if (room == null)
            {
                throw new Exception($"Room with ID {id} not found.");
            }

            return room;
        }
    }
}
