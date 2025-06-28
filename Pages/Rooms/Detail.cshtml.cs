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

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                Console.WriteLine("Fetching room details for ID: " + Id);
                room = await _roomsService.GetRoomByIdAsync(Id);
                if (room == null)
                {
                    ModelState.AddModelError(string.Empty, "Room not found.");
                }

                if (room.Roomimages != null && room.Roomimages.Count > 0)
                {
                    Roomimage = room.Roomimages.ToList();
                }

                return Page();
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log the error, show an error message)
                ModelState.AddModelError(string.Empty, "An error occurred while retrieving the room: " + ex.Message);
                return Page();
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
                TempData["ErrorMessage"] = "Phòng này hiện đang được sử dụng. Vui lòng chọn phòng khác.";
                return RedirectToPage("/Rooms/Detail", new { id = RoomId });
            }

            if (CheckInDate >= CheckOutDate)
            {
                TempData["ErrorMessage"] = "Ngày nhận phòng phải nhỏ hơn ngày trả phòng.";
                return RedirectToPage("/Rooms/Detail", new { id = RoomId });
            }

            if (CheckInDate < DateOnly.FromDateTime(DateTime.Now))
            {
                TempData["ErrorMessage"] = "Ngày nhận phòng phải lớn hơn hoặc bằng ngày hiện tại.";
                return RedirectToPage("/Rooms/Detail", new { id = RoomId });
            }

            if (CheckOutDate < DateOnly.FromDateTime(DateTime.Now))
            {
                TempData["ErrorMessage"] = "Ngày trả phòng phải lớn hơn hoặc bằng ngày hiện tại.";
                return RedirectToPage("/Rooms/Detail", new { id = RoomId });
            }

            var room = await _roomsService.GetRoomByIdAsync(RoomId);

            if (room == null)
            {
                return NotFound("Room not found.");
            }

            var wishList = HttpContext.Session.GetObject<List<BookingItem>>("wishlist") ?? new List<BookingItem>();

            if (wishList.Count == 5)
            {
                TempData["ErrorMessage"] = "Bạn chỉ có thể thêm tối đa 5 phòng vào danh sách yêu thích.";
                return RedirectToPage("/Rooms/Detail", new { id = RoomId });
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
