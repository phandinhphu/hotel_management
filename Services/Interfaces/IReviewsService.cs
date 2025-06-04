using Hotel_Management.Helpers;
using Hotel_Management.Models;

namespace Hotel_Management.Services.Interfaces
{
    public interface IReviewsService
    {
        Task<PaginatedList<Review>> GetPaginatedAsync(int rating = 0, int hotelId = 1, int pageIndex = 1, int pageSize = 10);
        Task<IEnumerable<Review>> GetAllAsync(int hotelId);
        Task<Review?> GetByUserAsync(string userId);
        Task<bool> AddAsync(Review review);
        Task<bool> DeleteAsync(int reviewId);
    }
}
