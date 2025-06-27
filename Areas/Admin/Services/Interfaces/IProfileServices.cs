using Hotel_Management.Areas.Admin.ViewModels;
using System.Security.Claims;

namespace Hotel_Management.Areas.Admin.Services.Interfaces
{
    public interface IProfileServices
    {
        /// <summary>
        /// Lấy thông tin cá nhân của user đang đăng nhập.
        /// </summary>
        /// <param name="user">ClaimsPrincipal của user hiện tại</param>
        /// <returns>ProfileVM chứa thông tin cá nhân</returns>
        ProfileVM GetProfile(ClaimsPrincipal user);

        /// <summary>
        /// Thay đổi thông tin cá nhân của user.
        /// </summary>
        /// <param name="model">ProfileVM chứa thông tin cập nhật</param>
        /// <param name="user">ClaimsPrincipal của user hiện tại</param>
        /// <returns>True nếu thành công, false nếu thất bại</returns>
        bool UpdateProfile(ProfileVM model, ClaimsPrincipal user);

        /// <summary>
        /// Thống kê hiệu suất làm việc của user (ví dụ: số lượng đơn xử lý, doanh thu, v.v.).
        /// </summary>
        /// <param name="user">ClaimsPrincipal của user hiện tại</param>
        /// <returns>PerformanceStatisticVM chứa dữ liệu thống kê</returns>
        List<ProfileVM.ChartDataPoint> GetPerformanceChartData(ClaimsPrincipal user, DateTime startDate, DateTime endDate);

    }
}
