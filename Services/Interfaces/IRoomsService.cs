﻿using Hotel_Management.Helpers;
using Hotel_Management.Models;

namespace Hotel_Management.Services.Interfaces
{
    public interface IRoomsService
    {
        public Task<PaginatedList<Room>> GetAllRoomsAsync(string status = "", int pageIndex = 1, int pageSize = 20);
        public Task<Room> GetRoomByIdAsync(int id);
    }
}
