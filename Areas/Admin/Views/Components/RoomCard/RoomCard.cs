using Microsoft.AspNetCore.Mvc;

namespace Hotel_Management.Areas.Admin.Views.Components.RoomCard
{
    public class RoomCard : ViewComponent
    {
        public async Task<IViewComponentResult> Invoke()
        {
            return View();
        }
    }
}
