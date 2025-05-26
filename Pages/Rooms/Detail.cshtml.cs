using Hotel_Management.Extensions;
using Hotel_Management.Models;
using Hotel_Management.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hotel_Management.Rooms.Pages
{
    public class DetailModel : PageModel
    {
        private readonly IRoomsService _roomsService;

        public DetailModel(IRoomsService roomsService)
        {
            _roomsService = roomsService;
        }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }
        public Room room { get; set; } = new Room();

        public void OnGet()
        {
            try
            {
                room = _roomsService.GetRoomByIdAsync(Id).Result;
                if (room == null)
                {
                    ModelState.AddModelError(string.Empty, "Room not found.");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log the error, show an error message)
                ModelState.AddModelError(string.Empty, "An error occurred while retrieving the room: " + ex.Message);
            }
        }

        public async Task<IActionResult> OnPostAddWishListAsync(
            int RoomId,
            string? RoomNumber,
            string? RoomType,
            string? RoomImage,
            decimal Price,
            DateTime CheckInDate,
            DateTime CheckOutDate
        )
        {
            Console.WriteLine($"RoomId: {RoomId}, RoomNumber: {RoomNumber}, RoomType: {RoomType}, RoomImage: {RoomImage}, Price: {Price}, CheckInDate: {CheckInDate}, CheckOutDate: {CheckOutDate}");
            var room = await _roomsService.GetRoomByIdAsync(RoomId);

            if (room == null)
            {
                ModelState.AddModelError(string.Empty, "Room not found.");
                return Page();
            }

            var wishList = HttpContext.Session.GetObject<List<BookingItem>>("wishlist") ?? new List<BookingItem>();

            wishList.Add(new BookingItem
            {
                RoomId = RoomId,
                RoomNumber = RoomNumber,
                RoomType = RoomType,
                RoomImage = RoomImage,
                Price = Price,
                CheckInDate = CheckInDate,
                CheckOutDate = CheckOutDate
            });

            HttpContext.Session.SetObject("wishlist", wishList);

            return RedirectToPage("/Bookings/WishList");
        }
    }
}
