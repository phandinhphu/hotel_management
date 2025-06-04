using Hotel_Management.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotel_Management.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Staff")]
    public class ReviewController : Controller
    {
        private readonly IReviewsService _reviewsService;

        public ReviewController(IReviewsService reviewsService)
        {
            _reviewsService = reviewsService;
        }

        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; } = 1;
        [BindProperty(SupportsGet = true)]
        public int Rating { get; set; } = 0;

        // [GET] /Admin/Review
        public async Task<IActionResult> Index()
        {
            var reviews = await _reviewsService.GetPaginatedAsync(Rating, 1, PageIndex);

            ViewBag.Rating = Rating;

            return View(reviews);
        }

        // [POST] /Admin/Review/Delete/{id}
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid review ID.");
            }

            var success = await _reviewsService.DeleteAsync(id);
            if (!success)
            {
                return NotFound("Review not found or could not be deleted.");
            }

            return RedirectToAction(nameof(Index), new { PageIndex, Rating });
        }
    }
}
