using Hotel_Management.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Hotel_Management.Pages.Bookings
{
    public class HistoryModel : PageModel
    {
        private readonly HotelManagementContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HistoryModel(HotelManagementContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<Booking> Bookings { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToPage(
                    "/Account/Login",
                    new { area = "Identity", returnUrl = Url.Page("/Bookings/History") }
                );
            }

            Bookings = await _context.Bookings
                .Where(b => b.UserId == userId)
                .Include(b => b.BookingsRoomDetails)
                    .ThenInclude(br => br.Room)
                .Include(b => b.BookingsServiceDetails)
                    .ThenInclude(bs => bs.Service)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            return Page();
        }
    }
}
