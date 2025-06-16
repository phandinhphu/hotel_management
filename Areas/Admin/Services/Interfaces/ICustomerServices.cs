using Hotel_Management.Helpers;
using Hotel_Management.Models;

namespace Hotel_Management.Areas.Admin.Services.Interfaces
{
    public interface ICustomerServices
    {
        Task<DateOnly?> getLastCheckInByIdAsync(string id);
        Task<Decimal?> getTotalSpentByIdAsync(string id);
    }
}
