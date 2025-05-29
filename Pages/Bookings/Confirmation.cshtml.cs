using Hotel_Management.Extensions;
using Hotel_Management.Hubs;
using Hotel_Management.Models;
using Hotel_Management.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Hotel_Management.Pages.Bookings
{
    public class ConfirmationModel : PageModel
    {
        private readonly ILogger<ConfirmationModel> _logger;
        private readonly HotelManagementContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRoomsService _roomsService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHubContext<NotificationHub> _hubContext;

        public ConfirmationModel(
            ILogger<ConfirmationModel> logger,
            HotelManagementContext context,
            UserManager<ApplicationUser> userManager,
            IRoomsService roomsService,
            SignInManager<ApplicationUser> signInManager,
            IHubContext<NotificationHub> hubContext)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _roomsService = roomsService;
            _signInManager = signInManager;
            _hubContext = hubContext;
        }

        public List<BookingItem> BookingItems { get; set; } = new List<BookingItem>();

        public void OnGet()
        {
            if (!_signInManager.IsSignedIn(User))
            {
                RedirectToPage("/Account/Login");
            }

            BookingItems = HttpContext.Session.GetObject<List<BookingItem>>("wishlist") ?? new List<BookingItem>();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            BookingItems = HttpContext.Session.GetObject<List<BookingItem>>("wishlist") ?? new List<BookingItem>();
            if (BookingItems.Count == 0)
            {
                return RedirectToPage("/Index");
            }

            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToPage("/Account/Login");
            }

            foreach (var item in BookingItems)
            {
                var room = await _roomsService.GetRoomByIdAsync(item.RoomId);

                var serviceList = await _context.Services
                    .Where(s => item.ServicesSelected.Contains(s.Id))
                    .ToListAsync();

                var totalPriceRooms = (item.CheckOutDate.DayNumber - item.CheckInDate.DayNumber) * room.Price;
                var totalPriceServices = serviceList.Sum(s => s.Price);

                var booking = new Booking
                {
                    UserId = userId,
                    TotalPriceRooms = totalPriceRooms,
                    TotalPriceServices = totalPriceServices,
                    TotalPrice = totalPriceRooms + totalPriceServices,
                    CreatedAt = DateTime.UtcNow,
                    Status = "Pending"
                };
                _context.Bookings.Add(booking);
                await _context.SaveChangesAsync();

                var bookingRoom = new BookingsRoomDetail
                {
                    BookingId = booking.Id,
                    RoomId = item.RoomId,
                    CheckIn = item.CheckInDate,
                    CheckOut = item.CheckOutDate,
                    Price = totalPriceRooms
                };
                _context.BookingsRoomDetails.Add(bookingRoom);
                await _context.SaveChangesAsync();

                foreach (var service in serviceList)
                {
                    var bookingService = new BookingsServiceDetail
                    {
                        BookingId = booking.Id,
                        ServiceId = service.Id,
                        Price = service.Price
                    };
                    _context.BookingsServiceDetails.Add(bookingService);
                    await _context.SaveChangesAsync();
                }
            }

            await _hubContext.Clients.Group("Staff")
                .SendAsync("ReceiveNotification", $"📥 Khách {_userManager.GetUserName(User)} vừa đặt phòng.");

            await _hubContext.Clients.Group("Admin")
                .SendAsync("ReceiveNotification", $"📥 Khách {_userManager.GetUserName(User)} vừa đặt phòng.");

            HttpContext.Session.Remove("wishlist");
            return RedirectToPage("/Bookings/Success");
        }
    }
}
