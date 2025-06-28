using System.Security.Claims;
using AutoMapper;
using Hotel_Management.Areas.Admin.ViewModels;
using Hotel_Management.Models;
using Hotel_Management.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hotel_Management.Pages.Reviews
{
    public class EditModel : PageModel
    {
        private readonly IReviewsService _reviewsService;
        private readonly IMapper _mapper;

        public EditModel(IReviewsService reviewsService, IMapper mapper)
        {
            _reviewsService = reviewsService;
            _mapper = mapper;
        }

        [BindProperty]
        public ReviewVM Review { get; set; } = new ReviewVM();
        [BindProperty(SupportsGet = true)]
        public int ReviewId { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var review = await _reviewsService.GetByIdAsync(ReviewId, User.FindFirstValue(ClaimTypes.NameIdentifier));
                if (review != null)
                {
                    Review = _mapper.Map<ReviewVM>(review);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Review not found.");
                }

                return Page();
            }
            catch (Exception ex)
            {
                return NotFound($"An error occurred while retrieving the review: {ex.Message}");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                var review = _mapper.Map<Review>(Review);
                var result = await _reviewsService.UpdateAsync(review, User.FindFirstValue(ClaimTypes.NameIdentifier));
                if (result)
                {
                    return RedirectToPage("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Failed to update the review.");
                    return Page();
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred while updating the review: {ex.Message}");
            }
        }
    }
}
