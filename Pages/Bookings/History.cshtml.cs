using Hotel_Management.Helpers;
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

        [BindProperty(SupportsGet = true)]
        public string StatusFilter { get; set; }

        public PaginatedList<Booking> Bookings { get; set; }

        public async Task<IActionResult> OnGetAsync(int pageIndex = 1)
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToPage(
                    "/Account/Login",
                    new { area = "Identity", returnUrl = Url.Page("/Bookings/History") }
                );
            }

            var bookingsQuery = _context.Bookings
                .Where(b => b.UserId == userId)
                .Include(b => b.BookingsRoomDetails)
                    .ThenInclude(br => br.Room)
                .Include(b => b.BookingsServiceDetails)
                    .ThenInclude(bs => bs.Service)
                .OrderByDescending(b => b.CreatedAt);

            if (!string.IsNullOrEmpty(StatusFilter) && StatusFilter != "All")
            {
                bookingsQuery = (IOrderedQueryable<Booking>)bookingsQuery.Where(b => b.Status == StatusFilter);
            }

            Bookings = await PaginatedList<Booking>.Create(
                    bookingsQuery.AsNoTracking(), pageIndex, 4);

            return Page();
        }

        public async Task<IActionResult> OnPostCancelAsync(int id)
        {
            var userId = _userManager.GetUserId(User);
            var booking = await _context.Bookings
                .Include(b => b.BookingsRoomDetails)
                .FirstOrDefaultAsync(b => b.Id == id && b.UserId == userId);

            if (booking == null || booking.Status != "Pending")
            {
                return NotFound();
            }

            booking.Status = "Cancelled";
            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();

            return RedirectToPage("History");
        }
    }
}
