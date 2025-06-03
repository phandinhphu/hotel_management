using Hotel_Management.Helpers;
using Hotel_Management.Models;
using Hotel_Management.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hotel_Management.Rooms.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IRoomsService _roomsService;

        public IndexModel(IRoomsService roomsService)
        {
            _roomsService = roomsService;
        }

        public PaginatedList<Room> Rooms { get; set; }
        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; } = 1;
        [BindProperty(SupportsGet = true)]
        public string? Status { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Rooms = await _roomsService.GetAllRoomsAsync(Status, PageIndex, 20);

            return Page();
        }
    }
}
