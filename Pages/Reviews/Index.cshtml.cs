using System.Security.Claims;
using AutoMapper;
using Hotel_Management.Areas.Admin.ViewModels;
using Hotel_Management.Models;
using Hotel_Management.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hotel_Management.Reviews.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IReviewsService _reviewsService;
        private readonly IMapper _mapper;

        public IndexModel(IReviewsService reviewsService, IMapper mapper)
        {
            _mapper = mapper;
            _reviewsService = reviewsService;
        }

        [BindProperty]
        public ReviewVM Review { get; set; } = new ReviewVM();

        public async Task<IActionResult> OnGetAsync()
        {
            Review = new ReviewVM()
            {
                HotelId = 1,
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
            };

            if (Review.UserId == null)
            {
                return RedirectToPage("Index");
            }

            var existingReview = await _reviewsService.GetByUserAsync(Review.UserId);

            if (existingReview != null)
            {
                Review = _mapper.Map<ReviewVM>(existingReview);

                return Page();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var review = _mapper.Map<Review>(Review);

            var result = await _reviewsService.AddAsync(review);

            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Failed to add review.");
            }

            return RedirectToPage("/Reviews/Index");
        }
    }
}
