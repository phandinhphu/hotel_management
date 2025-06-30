using Hotel_Management.Extensions;
using Hotel_Management.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hotel_Management.Pages.Bookings
{
    public class WishListModel : PageModel
    {
        private readonly ILogger<WishListModel> _logger;

        public WishListModel(ILogger<WishListModel> logger)
        {
            _logger = logger;
        }

        [BindProperty]
        public List<BookingItem> BookingItems { get; set; } = new List<BookingItem>();

        public void OnGet()
        {
            BookingItems = HttpContext.Session.GetObject<List<BookingItem>>("wishlist") ?? new List<BookingItem>();
        }

        public IActionResult OnPost()
        {
            // Save to wishlist in session
            HttpContext.Session.SetObject("wishlist", BookingItems);

            return RedirectToPage("/Bookings/Confirmation");
        }
    }
}
