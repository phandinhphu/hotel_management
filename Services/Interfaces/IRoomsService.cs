using Hotel_Management.Helpers;
using Hotel_Management.Models;

namespace Hotel_Management.Services.Interfaces
{
    public interface IRoomsService
    {
        public Task<PaginatedList<Room>> GetAllRoomsAsync(string roomNumber = "", int pageIndex = 1, int pageSize = 20);
        public Task<Room> GetRoomsByIdAsync(int id);
    }
}
