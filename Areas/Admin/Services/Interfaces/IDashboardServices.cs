using Hotel_Management.Areas.Admin.ViewModels;
namespace Hotel_Management.Areas.Admin.Services.Interfaces
{
    public interface IDashboardServices
    {

        #region Summary
        /// <summary>
        /// Lấy tổng số đặt phòng và thông tin tăng trưởng
        /// </summary>
        /// <param name="startDate">Ngày bắt đầu</param>
        /// <param name="endDate">Ngày kết thúc</param>
        /// <returns>Tuple chứa (Tổng số đặt phòng, Tỷ lệ tăng trưởng, Có phải tăng trưởng)</returns>
        int GetTotalBookings(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Lấy tổng số khách hàng
        /// </summary>
        /// <returns>Tổng số khách hàng</returns>
        int GetTotalCustomers();

        /// <summary>
        /// Lấy tổng doanh thu dịch vụ và thông tin tăng trưởng
        /// </summary>
        /// <param name="startDate">Ngày bắt đầu</param>
        /// <param name="endDate">Ngày kết thúc</param>
        /// <returns>Tuple chứa (Tổng doanh thu dịch vụ, Tỷ lệ tăng trưởng, Có phải tăng trưởng)</returns>
        decimal GetTotalServiceRevenue(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Lấy tổng doanh thu phòng và thông tin tăng trưởng
        /// </summary>
        /// <param name="startDate">Ngày bắt đầu</param>
        /// <param name="endDate">Ngày kết thúc</param>
        /// <returns>Tuple chứa (Tổng doanh thu phòng, Tỷ lệ tăng trưởng, Có phải tăng trưởng)</returns>
        decimal GetTotalRoomRevenue(DateTime startDate, DateTime endDate);
        #endregion

        #region Chart
        /// <summary>
        /// Lấy dữ liệu biểu đồ doanh thu
        /// </summary>
        /// <param name="startDate">Ngày bắt đầu</param>
        /// <param name="endDate">Ngày kết thúc</param>
        /// <returns>Danh sách dữ liệu biểu đồ doanh thu</returns>
        List<DashboardVM.ChartDataPoint> GetRevenueChartData(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Lấy dữ liệu biểu đồ đặt phòng
        /// </summary>
        /// <param name="startDate">Ngày bắt đầu</param>
        /// <param name="endDate">Ngày kết thúc</param>
        /// <returns>Danh sách dữ liệu biểu đồ đặt phòng</returns>
        List<DashboardVM.ChartDataPoint> GetBookingChartData(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Lấy dữ liệu biểu đồ loại phòng
        /// </summary>
        /// <param name="startDate">Ngày bắt đầu</param>
        /// <param name="endDate">Ngày kết thúc</param>
        /// <returns>Danh sách dữ liệu biểu đồ loại phòng</returns>
        List<DashboardVM.ChartDataPoint> GetRoomTypeChartData(DateTime startDate, DateTime endDate);

        #endregion

        #region List
        /// <summary>
        /// Lấy danh sách doanh thu dịch vụ
        /// </summary>
        /// <param name="startDate">Ngày bắt đầu</param>
        /// <param name="endDate">Ngày kết thúc</param>
        /// <returns>Danh sách doanh thu dịch vụ</returns>
        List<DashboardVM.ServiceRevenueItem> GetServiceRevenueItems(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Lấy danh sách doanh thu loại phòng
        /// </summary>
        /// <param name="startDate">Ngày bắt đầu</param>
        /// <param name="endDate">Ngày kết thúc</param>
        /// <returns>Danh sách doanh thu loại phòng</returns>
        List<DashboardVM.RoomTypeRevenueItem> GetRoomTypeRevenueItems(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Lấy danh sách đánh giá của người dùng
        /// </summary>
        /// <param name="startDate">Ngày bắt đầu</param>
        /// <param name="endDate">Ngày kết thúc</param>
        /// <returns>Danh sách đánh giá của người dùng</returns>
        List<DashboardVM.UserReview> GetUserReviews(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Lấy danh sách thông tin nhân viên
        /// </summary>
        /// <returns>Danh sách thông tin nhân viên</returns>
        List<DashboardVM.StaffInfo> GetStaffInfos();

        /// <summary>
        /// Lấy danh sách loại phòng phổ biến
        /// </summary>
        /// <param name="startDate">Ngày bắt đầu</param>
        /// <param name="endDate">Ngày kết thúc</param>
        /// <returns>Danh sách loại phòng phổ biến</returns>
        List<DashboardVM.PopularRoomType> GetPopularRoomTypes(DateTime startDate, DateTime endDate);
        #endregion

        /// <summary>
        /// Lấy tất cả dữ liệu cho dashboard
        /// </summary>
        /// <param name="startDate">Ngày bắt đầu</param>
        /// <param name="endDate">Ngày kết thúc</param>
        /// <returns>View model dashboard đầy đủ</returns>
        Task<DashboardVM> GetDashboardDataAsync(DateTime startDate, DateTime endDate);

        #region Export Excel
        /// <summary>
        /// Lấy danh sách doanh thu dịch vụ để xuất ra Excel
        /// </summary>
        /// <param name="startDate">Ngày bắt đầu</param>
        /// <param name="endDate">Ngày kết thúc</param>
        /// <returns>Danh sách doanh thu dịch vụ</returns>
        List<ServiceRevenueExcel> GetServiceRevenueExcelData(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Lấy danh sách doanh thu phòng để xuất ra Excel
        /// </summary>
        /// <param name="startDate">Ngày bắt đầu</param>
        /// <param name="endDate">Ngày kết thúc</param>
        /// <returns>Danh sách doanh thu phòng</returns>
        List<RoomRevenueExcel> GetRoomRevenueExcelData(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Lấy danh sách doanh thu tổng hợp (phòng và dịch vụ) để xuất ra Excel
        /// </summary>
        /// <param name="startDate">Ngày bắt đầu</param>
        /// <param name="endDate">Ngày kết thúc</param>
        /// <returns>Danh sách doanh thu tổng hợp</returns>
        List<RevenueExcel> GetRevenueExcelData(DateTime startDate, DateTime endDate);

        #endregion
    }
}
