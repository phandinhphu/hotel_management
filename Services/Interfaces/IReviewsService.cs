using Hotel_Management.Models;

namespace Hotel_Management.Services.Interfaces
{
    public interface IReviewsService
    {
        Task<IEnumerable<Review>> GetAllAsync(int hotelId);
        Task<Review?> GetByUserAsync(string userId);
        Task<bool> AddAsync(Review review);
    }
}
