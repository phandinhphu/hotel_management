using Hotel_Management.Helpers;
using Hotel_Management.Models;

namespace Hotel_Management.Areas.Admin.Services.Interfaces
{
    public interface IStaffServices
    {
        Task<int> getTotalBookedRoomsByIdAsync(string id);
    }
}
