using Hotel_Management.Areas.Admin.ViewModels;

namespace Hotel_Management.Areas.Admin.Services.Interfaces
{
    public interface IExcelServices
    {
        /// <summary>
        /// Xuất dữ liệu doanh thu dịch vụ ra file Excel
        /// </summary>
        /// <param name="data">Dữ liệu doanh thu dịch vụ</param>
        /// <param name="startDate">Ngày bắt đầu</param>
        /// <param name="endDate">Ngày kết thúc</param>
        /// <returns>Mảng byte của file Excel</returns>
        byte[] ExportServiceRevenueToExcel(List<ServiceRevenueExcel> data, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Xuất dữ liệu doanh thu phòng ra file Excel
        /// </summary>
        /// <param name="data">Dữ liệu doanh thu phòng</param>
        /// <param name="startDate">Ngày bắt đầu</param>
        /// <param name="endDate">Ngày kết thúc</param>
        /// <returns>Mảng byte của file Excel</returns>
        byte[] ExportRoomRevenueToExcel(List<RoomRevenueExcel> data, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Xuất dữ liệu doanh thu tổng hợp ra file Excel
        /// </summary>
        /// <param name="data">Dữ liệu doanh thu tổng hợp</param>
        /// <param name="startDate">Ngày bắt đầu</param>
        /// <param name="endDate">Ngày kết thúc</param>
        /// <returns>Mảng byte của file Excel</returns>
        byte[] ExportRevenueToExcel(List<RevenueExcel> data, DateTime startDate, DateTime endDate);
    }
}
