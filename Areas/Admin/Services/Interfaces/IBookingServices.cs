using Hotel_Management.Areas.Admin.ViewModels;
using Hotel_Management.Helpers;
using Hotel_Management.Models;

namespace Hotel_Management.Areas.Admin.Services.Interfaces
{
    public interface IBookingServices
    {
        Task<PaginatedList<Booking>> GetBookingsAsync(string searchString = "", int pageIndex = 1, int pageSize = 20);
        Task<Booking> GetBookingByIdAsync(int id);
        Task<Booking> GetBookingByUserAsync(string userName);
        void ApproveBooking(int id, string userId);
        void RejectBooking(int id, string userId);
    }
}
