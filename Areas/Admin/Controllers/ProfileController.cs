using Hotel_Management.Areas.Admin.Services.Interfaces;
using Hotel_Management.Areas.Admin.ViewModels;
using Hotel_Management.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotel_Management.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Staff")]
    public class ProfileController : Controller
    {
        private readonly IProfileServices _profileServices;

        public ProfileController(IProfileServices profileServices)
        {
            _profileServices = profileServices;
        }

        public IActionResult Index()
        {
            try
            {
                var profile = _profileServices.GetProfile(User);

                DateTime startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                DateTime endDate = DateTime.Now;

                profile.PerformanceChartData = _profileServices.GetPerformanceChartData(User, startDate, endDate);

                return View(profile);
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                // _logger.LogError(ex, "Error loading profile data");
                // Return an empty profile view model
                return View(new ProfileVM
                {
                    UserName = "",
                    Email = "",
                    PhoneNumber = "",
                    Password = "",
                    ConfirmPassword = "",
                    PerformanceChartData = new List<ProfileVM.ChartDataPoint>()
                });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateProfile(ProfileVM model)
        {
            var result = _profileServices.UpdateProfile(model, User);

            if (result)
                TempData["SuccessMessage"] = $"Cập nhật thông tin cá nhân thành công!";
            else TempData["ErrorMessage"] = $"Cập nhật thông tin cá nhân thất bại!";

            return RedirectToAction(nameof(Index));
        }
    }
}
