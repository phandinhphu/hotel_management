using Hotel_Management.Helpers;
using Hotel_Management.Models;
using Microsoft.EntityFrameworkCore;

namespace Hotel_Management.Areas.Admin.Services
{
    public interface IRoomService
    {
        Task<PaginatedList<Room>> GetAllAsync(int pageIndex = 1, int pageSize = 20);
        Task<Room> GetByIdAsync(int id);
    }
    public class RoomService : IRoomService
    {
        private readonly HotelManagementContext _context;
        public RoomService(HotelManagementContext context)
        {
            _context = context;
        }
        #region Async Methods

        /// <summary>
        /// Lấy toàn bộ danh sách phòng
        /// </summary>
        /// <param name="roomNumber"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<PaginatedList<Room>> GetAllAsync( int pageIndex = 1, int pageSize = 20)
        {
            var query = _context.Rooms.AsQueryable();

            return await PaginatedList<Room>.Create(
                query.Include(r => r.Roomimages),
                pageIndex,
                pageSize
            );
        }

        /// <summary>
        /// Lấy phòng theo ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Room> GetByIdAsync(int id)
        {
            var room = await _context.Rooms
                .Include(r => r.Roomimages)
                .Where(r => r.Id == id)
                .FirstOrDefaultAsync();
            if (room == null)
            {
                throw new Exception($"Room with ID {id} not found.");
            }
            return room;
        }
        #endregion
    }
}
