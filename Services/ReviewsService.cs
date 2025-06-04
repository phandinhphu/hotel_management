using Hotel_Management.Helpers;
using Hotel_Management.Models;
using Hotel_Management.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hotel_Management.Services
{
    public class ReviewsService : IReviewsService
    {
        private readonly HotelManagementContext _context;

        public ReviewsService(HotelManagementContext context)
        {
            _context = context;
        }

        public async Task<PaginatedList<Review>> GetPaginatedAsync(int rating = 0, int hotelId = 1, int pageIndex = 1, int pageSize = 10)
        {
            if (hotelId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(hotelId), "Hotel ID must be greater than zero.");
            }

            var query = _context.Reviews
                .Where(r => r.HotelId == hotelId)
                .Include(r => r.User)
                .AsQueryable();

            if (rating > 0)
            {
                query = query.Where(r => r.Rating == rating);
            }

            return await PaginatedList<Review>.Create(query, pageIndex, pageSize);
        }

        public async Task<bool> AddAsync(Review review)
        {
            if (review == null)
            {
                throw new ArgumentNullException(nameof(review));
            }
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            // Update rating for the hotel
            if (review.HotelId.HasValue)
            {
                var hotel = await _context.Hotels.FindAsync(review.HotelId.Value);
                if (hotel != null)
                {
                    hotel.Rating = (float?)(_context.Reviews
                        .Where(r => r.HotelId == review.HotelId)
                        .Average(r => r.Rating) ?? 0);
                    _context.Hotels.Update(hotel);
                }
            }

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<Review>> GetAllAsync(int hotelId)
        {
            if (hotelId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(hotelId), "Hotel ID must be greater than zero.");
            }

            return await _context.Reviews
                .Where(r => r.HotelId == hotelId)
                .Include(r => r.User)
                .ToListAsync();
        }

        public Task<Review?> GetByUserAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId), "User ID cannot be null or empty.");
            }

            return _context.Reviews
                .Where(r => r.UserId == userId)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> DeleteAsync(int reviewId)
        {
            if (reviewId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(reviewId), "Review ID must be greater than zero.");
            }

            var review = await _context.Reviews.FindAsync(reviewId);
            if (review == null)
            {
                return false; // Review not found
            }

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            // Update rating for the hotel
            if (review.HotelId.HasValue)
            {
                var hotel = await _context.Hotels.FindAsync(review.HotelId.Value);
                if (hotel != null)
                {
                    hotel.Rating = (float?)(_context.Reviews
                        .Where(r => r.HotelId == review.HotelId)
                        .Average(r => r.Rating) ?? 0);
                    _context.Hotels.Update(hotel);
                }
            }

            return true;
        }
    }
}
