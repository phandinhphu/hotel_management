using Hotel_Management.Extensions;
using Hotel_Management.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hotel_Management.Pages.Bookings
{
    public class ConfirmationModel : PageModel
    {
        private readonly ILogger<ConfirmationModel> _logger;

        public ConfirmationModel(ILogger<ConfirmationModel> logger)
        {
            _logger = logger;
        }

        public List<BookingItem> BookingItems { get; set; } = new List<BookingItem>();

        public void OnGet()
        {
            BookingItems = HttpContext.Session.GetObject<List<BookingItem>>("wishlist") ?? new List<BookingItem>();
        }
    }
}
