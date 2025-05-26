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

        public IEnumerable<Room> Rooms { get; set; } = new List<Room>();

        public void OnGet()
        {
            try
            {
                Rooms = _roomsService.GetAllRoomsAsync().Result;
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log the error, show an error message)
                ModelState.AddModelError(string.Empty, "An error occurred while retrieving rooms: " + ex.Message);
            }
        }
    }
}
