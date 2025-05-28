using Microsoft.EntityFrameworkCore;
using Hotel_Management.Helpers;
using Hotel_Management.Models;
using Hotel_Management.Areas.Admin.ViewModels;
using Hotel_Management.Areas.Admin.Services.Interfaces;

namespace Hotel_Management.Areas.Admin.Services
{
    public class RoomService : IRoomService
    {
        private readonly HotelManagementContext _context;
        public RoomService(HotelManagementContext context)
        {
            _context = context;
        }
        #region Async Methods

        public async Task<PaginatedList<RoomVM>> GetAllAsync(
            string searchTerm = "", 
            string roomType = "", 
            string status = "", 
            decimal? minPrice = null, 
            decimal? maxPrice = null, 
            int pageIndex = 1, 
            int pageSize = 20)
        {
            var query = _context.Rooms.AsNoTracking();
            
            // Search
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.Trim().ToLower();
                query = query.Where(r => 
                    (r.RoomNumber != null && r.RoomNumber.ToLower().Contains(searchTerm)) ||
                    (r.Type != null && r.Type.ToLower().Contains(searchTerm)) ||
                    (r.Description != null && r.Description.ToLower().Contains(searchTerm))
                );
            }
            // Filters
            if (!string.IsNullOrWhiteSpace(roomType))
            {
                query = query.Where(r => r.Type == roomType);
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(r => r.Status == status);
            }

            if (minPrice.HasValue)
            {
                query = query.Where(r => r.Price >= minPrice);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(r => r.Price <= maxPrice);
            }
            
            var mappedQuery = query.Select(r => new RoomVM
            {
                ID = r.Id,
                HotelId = r.HotelId ?? 0,
                RoomNumber = r.RoomNumber ?? "",
                Type = r.Type ?? "",
                Price = r.Price ?? 0,
                Status = r.Status ?? "",
                Description = r.Description ?? "",
                Image = r.Image ?? ""
            });
            
            return await PaginatedList<RoomVM>.Create(
                mappedQuery,
                pageIndex,
                pageSize
            );
        }

        public async Task<RoomVM> GetByIdAsync(int id)
        {
            var room = await _context.Rooms
                .AsNoTracking()
                .Where(r => r.Id == id)
                .Select(r => new RoomVM
                {
                    ID = r.Id,
                    HotelId = r.HotelId ?? 0,
                    RoomNumber = r.RoomNumber ?? "",
                    Type = r.Type ?? "",
                    Price = r.Price ?? 0,
                    Status = r.Status ?? "",
                    Description = r.Description ?? "",
                    Image = r.Image ?? ""
                }).FirstOrDefaultAsync();
            if (room == null)
            {
                throw new KeyNotFoundException($"Room with ID {id} not found.");
            }
            return room;
        }

        public async Task<List<string>> GetAllRoomTypesAsync()
        {
            var types = await _context.Rooms
                .AsNoTracking()
                .Where(r => r.Type != null && r.Type != "")
                .Select(r => r.Type!)
                .Distinct()
                .ToListAsync();

            if (types == null || types.Count == 0)
            {
                throw new InvalidOperationException("No room types found.");
            }

            return types;
        }
        public async Task<int> CreateAsync(RoomVM roomVM)
        {
            // Map ViewModel to entity
            var room = new Room
            {
                HotelId = roomVM.HotelId,
                RoomNumber = roomVM.RoomNumber,
                Type = roomVM.Type,
                Description = roomVM.Description,
                Price = roomVM.Price,
                Status = roomVM.Status,
                Capacity = roomVM.Capacity,
                Image = roomVM.Image
            };

            // Add to database
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();

            return room.Id;
        }

        #endregion
    }
}
