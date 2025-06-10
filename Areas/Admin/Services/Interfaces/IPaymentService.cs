using Hotel_Management.Helpers;
using Hotel_Management.Areas.Admin.ViewModels;

namespace Hotel_Management.Areas.Admin.Services.Interfaces
{
    public interface IPaymentService
    {
        /// <summary>
        /// Lấy danh sách thanh toán
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<PaginatedList<PaymentVM>> GetAllAsync(
            string searchTerm = "",
            int pageIndex = 1,
            int pageSize = 20);
        /// <summary>
        /// Lấy thông tin thanh toán theo ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<PaymentVM> GetByIdAsync(int id);

        /// <summary>
        /// Xóa thanh toán theo ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(int id);
    }
}
