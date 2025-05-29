using AutoMapper;
using Hotel_Management.Areas.Admin.Services.Interfaces;
using Hotel_Management.Areas.Admin.ViewModels;
using Hotel_Management.Helpers;
using Hotel_Management.Models;
using Microsoft.EntityFrameworkCore;

namespace Hotel_Management.Areas.Admin.Services
{
    public enum BookingStatus
    {
        UnPaid,
        Rejected
    }

    public class BookingServices : IBookingServices
    {
        private readonly HotelManagementContext _context;
        private readonly IMapper _mapper;

        public BookingServices(HotelManagementContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void ApproveBooking(int id)
        {
            var booking = GetBookingByIdAsync(id).Result;

            if (booking == null)
            {
                throw new KeyNotFoundException($"Booking with ID {id} not found.");
            }

            booking.Status = BookingStatus.UnPaid.ToString();

            // Chuyển trạng thái phòng trang Occupied
            foreach (var roomDetail in booking.BookingsRoomDetails)
            {
                Console.WriteLine("roomDetail" + roomDetail.RoomId);
                var room = _context.Rooms.Find(roomDetail.RoomId);
                if (room != null)
                {
                    room.Status = "Occupied";
                    _context.Rooms.Update(room);
                }
            }

            _context.SaveChanges();
        }

        public async Task<Booking> GetBookingByIdAsync(int id)
        {
            var booking = await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Staff)
                .Include(b => b.BookingsRoomDetails)
                    .ThenInclude(brd => brd.Room)
                .Include(b => b.BookingsServiceDetails)
                    .ThenInclude(bsd => bsd.Service)
                .FirstOrDefaultAsync(b => b.Id == id);

            return booking ?? throw new KeyNotFoundException($"Booking with ID {id} not found.");
        }

        public Task<Booking> GetBookingByUserAsync(string userName)
        {
            throw new NotImplementedException();
        }

        public async Task<PaginatedList<Booking>> GetBookingsAsync(string searchString = "", int pageIndex = 1, int pageSize = 20)
        {
            var query = _context.Bookings
                .Include(b => b.User)
                .Include(b => b.BookingsRoomDetails)
                    .ThenInclude(brd => brd.Room)
                .Include(b => b.BookingsServiceDetails)
                    .ThenInclude(bsd => bsd.Service)
                .OrderByDescending(b => b.CreatedAt)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(b => b.BookingsRoomDetails.Any(brd => 
                            brd.Room.RoomNumber.Contains(searchString, StringComparison.OrdinalIgnoreCase)));
            }

            return await PaginatedList<Booking>.Create(
                query, pageIndex, pageSize);
        }

        public void RejectBooking(int id)
        {
            var booking = _context.Bookings.Find(id);

            if (booking == null)
            {
                throw new KeyNotFoundException($"Booking with ID {id} not found.");
            }

            booking.Status = BookingStatus.Rejected.ToString();

            _context.Bookings.Update(booking);
            _context.SaveChanges();
        }
    }
}
