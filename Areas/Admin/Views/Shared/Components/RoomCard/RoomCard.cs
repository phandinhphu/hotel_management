using Microsoft.AspNetCore.Mvc;
using Hotel_Management.Areas.Admin.ViewModels;
using Hotel_Management.Helpers;

namespace Hotel_Management.Areas.Admin.Views.Shared.Components.RoomCard
{
    public class RoomCard : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(RoomVM room)
        {
            return View(room);
        }
    }
}
