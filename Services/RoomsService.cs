using Hotel_Management.Helpers;
using Hotel_Management.Models;
using Hotel_Management.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hotel_Management.Services
{
    public class RoomsService : IRoomsService
    {
        private readonly HotelManagementContext _context;
        public IEnumerable<Room> MorkRooms { get; set; } = new List<Room>()
        {
            new Room
            {
                Id = 1,
                HotelId = 1,
                RoomNumber = "101",
                Type = "Deluxe",
                Description = "A spacious deluxe room with a king-size bed.",
                Price = 150.00m,
                Status = "Available",
                Capacity = 2,
                Image = "deluxe_room.jpg"
            },
            new Room
            {
                Id = 2,
                HotelId = 1,
                RoomNumber = "102",
                Type = "Standard",
                Description = "A cozy standard room with a queen-size bed.",
                Price = 100.00m,
                Status = "Occupied",
                Capacity = 2,
                Image = "standard_room.jpg"
            },
            new Room
            {
                Id = 3,
                HotelId = 1,
                RoomNumber = "103",
                Type = "Suite",
                Description = "A luxurious suite with a separate living area.",
                Price = 250.00m,
                Status = "Available",
                Capacity = 4,
                Image = "suite_room.jpg"
            },
            new Room
            {
                Id = 4,
                HotelId = 1,
                RoomNumber = "104",
                Type = "Family",
                Description = "A spacious family room with two queen beds.",
                Price = 200.00m,
                Status = "Available",
                Capacity = 4,
                Image = "family_room.jpg"
            },
            new Room
            {
                Id = 5,
                HotelId = 1,
                RoomNumber = "105",
                Type = "Economy",
                Description = "A budget-friendly economy room with basic amenities.",
                Price = 80.00m,
                Status = "Available",
                Capacity = 2,
                Image = "economy_room.jpg"
            },
            new Room
            {
                Id = 6,
                HotelId = 1,
                RoomNumber = "106",
                Type = "Business",
                Description = "A business room with a work desk and high-speed internet.",
                Price = 120.00m,
                Status = "Available",
                Capacity = 2,
                Image = "business_room.jpg"
            },
            new Room
            {
                Id = 7,
                HotelId = 1,
                RoomNumber = "107",
                Type = "Penthouse",
                Description = "A luxurious penthouse with stunning city views.",
                Price = 500.00m,
                Status = "Available",
                Capacity = 6,
                Image = "penthouse_room.jpg"
            },
            new Room
            {
                Id = 8,
                HotelId = 1,
                RoomNumber = "108",
                Type = "Accessible",
                Description = "An accessible room designed for guests with disabilities.",
                Price = 90.00m,
                Status = "Available",
                Capacity = 2,
                Image = "accessible_room.jpg"
            },
        };

        public RoomsService(HotelManagementContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Room>> GetAllRoomsAsync(string roomNumber = "", int pageIndex = 1, int pageSize = 20)
        {
            // Fix: Use Task.FromResult to wrap the result in a Task since MorkRooms is not an asynchronous source.
            var filteredRooms = MorkRooms.Where(r => string.IsNullOrEmpty(roomNumber) || (r.RoomNumber != null && r.RoomNumber.Contains(roomNumber)))
                                         .Skip((pageIndex - 1) * pageSize)
                                         .Take(pageSize);

            return await Task.FromResult(filteredRooms);
        }

        //public async Task<PaginatedList<Room>> GetAllRoomsAsync(string roomNumber = "", int pageIndex = 1, int pageSize = 20)
        //{
        //    var query = _context.Rooms.AsQueryable();

        //    if (!string.IsNullOrEmpty(roomNumber))
        //    {
        //        query = query.Where(r => r.RoomNumber != null && r.RoomNumber.Contains(roomNumber));
        //    }

        //    return await PaginatedList<Room>.Create(
        //        query.Include(r => r.Roomimages),
        //        pageIndex, 
        //        pageSize
        //    );
        //}

        public async Task<Room> GetRoomByIdAsync(int id)
        {
            //var room = await _context.Rooms
            //    .Include(r => r.Roomimages)
            //    .Where(r => r.HotelId == id)
            //    .FirstOrDefaultAsync();

            var room = MorkRooms.FirstOrDefault(r => r.Id == id);

            if (room == null)
            {
                throw new Exception($"Room with ID {id} not found.");
            }

            return await Task.FromResult(room);
        }
    }
}
