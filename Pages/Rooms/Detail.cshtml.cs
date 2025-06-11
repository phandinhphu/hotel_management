using Hotel_Management.Extensions;
using Hotel_Management.Models;
using Hotel_Management.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hotel_Management.Rooms.Pages
{
    public class DetailModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IRoomsService _roomsService;

        public DetailModel(SignInManager<ApplicationUser> signInManager,IRoomsService roomsService)
        {
            _signInManager = signInManager;
            _roomsService = roomsService;
        }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }
        public Room room { get; set; } = new Room();
        public List<Roomimage> Roomimage { get; set; } = new List<Roomimage>();

        public void OnGet()
        {
            try
            {
                room = _roomsService.GetRoomByIdAsync(Id).Result;
                if (room == null)
                {
                    ModelState.AddModelError(string.Empty, "Room not found.");
                }

                if (room.Roomimages != null && room.Roomimages.Count > 0)
                {
                    Roomimage = room.Roomimages.ToList();
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
            DateOnly CheckInDate,
            DateOnly CheckOutDate,
            String Status = "Available"
        )
        {
            if (!_signInManager.IsSignedIn(User))
            {
                return RedirectToPage(
                    "/Account/Login",
                    new { area = "Identity", returnUrl = Url.Page("/Rooms/Detail", new { id = RoomId }) }
                );
            }

            if (Status == "Occupied")
            {
                // Window alert to inform user that the room is occupied
                ModelState.AddModelError(string.Empty, "Phòng này hiện đang được sử dụng. Vui lòng chọn phòng khác hoặc quay lại sau.");
                return Page();
            }

            if (CheckInDate >= CheckOutDate)
            {
                ModelState.AddModelError(string.Empty, "Ngày trả phòng phải lớn hơn ngày nhận phòng.");
                return Page();
            }

            if (CheckInDate < DateOnly.FromDateTime(DateTime.Now))
            {
                ModelState.AddModelError(string.Empty, "Ngày nhận phòng phải lớn hơn hoặc bằng ngày hiện tại.");
                return Page();
            }

            if (CheckOutDate < DateOnly.FromDateTime(DateTime.Now))
            {
                ModelState.AddModelError(string.Empty, "Ngày trả phòng phải lớn hơn hoặc bằng ngày hiện tại.");
                return Page();
            }

            var room = await _roomsService.GetRoomByIdAsync(RoomId);

            if (room == null)
            {
                ModelState.AddModelError(string.Empty, "Room not found.");
                return Page();
            }

            var wishList = HttpContext.Session.GetObject<List<BookingItem>>("wishlist") ?? new List<BookingItem>();

            if (wishList.Count == 5)
            {
                ModelState.AddModelError(string.Empty, "You can only add up to 5 rooms to your wishlist.");
                return Page();
            }

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
