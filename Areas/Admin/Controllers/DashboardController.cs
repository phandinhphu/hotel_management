using Microsoft.AspNetCore.Mvc;
using Hotel_Management.Areas.Admin.Services.Interfaces;
using Hotel_Management.Areas.Admin.ViewModels;
using Hotel_Management.Areas.Admin.Services;

namespace Hotel_Management.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        private readonly IDashboardServices _dashboardServices;
        private readonly IExcelServices _excelServices;
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(IDashboardServices dashboardServices, IExcelServices excelServices, ILogger<DashboardController> logger)
        {
            _dashboardServices = dashboardServices;
            _excelServices = excelServices;
            _logger = logger;
        }

        public async Task<IActionResult> Index(DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                // Default to current month if no dates provided
                startDate ??= new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                endDate ??= DateTime.Now;

                var dashboardData = await _dashboardServices.GetDashboardDataAsync(startDate.Value, endDate.Value);
                return View(dashboardData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading dashboard data");
                // Initialize with empty data instead of null
                var emptyModel = new DashboardVM
                {
                    StartDate = startDate ?? DateTime.Now,
                    EndDate = endDate ?? DateTime.Now,
                    RevenueChartData = new List<DashboardVM.ChartDataPoint>(),
                    BookingChartData = new List<DashboardVM.ChartDataPoint>(),
                    RoomTypeChartData = new List<DashboardVM.ChartDataPoint>()
                };
                return View(emptyModel);
            }
        }

        public IActionResult ServiceExcelPreview(DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                // Default to current month if no dates provided
                startDate ??= new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                endDate ??= DateTime.Now;

                var serviceRevenueData = _dashboardServices.GetServiceRevenueExcelData(startDate.Value, endDate.Value);

                var excelVM = new ExcelVM
                {
                    Title = "Báo Cáo Doanh Thu Dịch Vụ",
                    GeneratedDate = DateTime.Now,
                    GeneratedBy = User.Identity?.Name ?? "System",
                    StartDate = startDate.Value,
                    EndDate = endDate.Value,
                    ServiceRevenues = serviceRevenueData
                };

                return View(excelVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting dashboard data to Excel");
                return BadRequest("An error occurred while exporting data.");
            }
        }

        public IActionResult RoomExcelPreview(DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                // Default to current month if no dates provided
                startDate ??= new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                endDate ??= DateTime.Now;

                var RoomRevenueExcel = _dashboardServices.GetRoomRevenueExcelData(startDate.Value, endDate.Value);

                var excelVM = new ExcelVM
                {
                    Title = "Báo Cáo Doanh Thu Phòng",
                    GeneratedDate = DateTime.Now,
                    GeneratedBy = User.Identity?.Name ?? "System",
                    StartDate = startDate.Value,
                    EndDate = endDate.Value,
                    RoomRevenue = RoomRevenueExcel
                };

                return View(excelVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting dashboard data to Excel");
                return BadRequest("An error occurred while exporting data.");
            }
        }

        public IActionResult RevenueExcelPreview(DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                // Default to current month if no dates provided
                startDate ??= new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                endDate ??= DateTime.Now;
                var revenueData = _dashboardServices.GetRevenueExcelData(startDate.Value, endDate.Value);
                var excelVM = new ExcelVM
                {
                    Title = "Báo Cáo Doanh Thu Tổng Hợp",
                    GeneratedDate = DateTime.Now,
                    GeneratedBy = User.Identity?.Name ?? "System",
                    StartDate = startDate.Value,
                    EndDate = endDate.Value,
                    Revenue = revenueData
                };
                return View(excelVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting dashboard data to Excel");
                return BadRequest("An error occurred while exporting data.");
            }
        }

        public IActionResult ExportToExcel(string section = "default", DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                // Default to current month if no dates provided
                startDate ??= new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                endDate ??= DateTime.Now;

                if (section == "total")
                {
                    var revenueData = _dashboardServices.GetRevenueExcelData(startDate.Value, endDate.Value);
                    var excelData = _excelServices.ExportRevenueToExcel(revenueData, startDate.Value, endDate.Value);
                    return File(
                        excelData,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        $"Revenue-{startDate:yyyy-MM-dd}-to-{endDate:yyyy-MM-dd}.xlsx");
                }
                else if (section == "service")
                {
                    // Export service revenue data to Excel
                    var serviceRevenueData = _dashboardServices.GetServiceRevenueExcelData(startDate.Value, endDate.Value);
                    var excelData = _excelServices.ExportServiceRevenueToExcel(serviceRevenueData, startDate.Value, endDate.Value);
                    return File(
                        excelData,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        $"ServiceRevenue-{startDate:yyyy-MM-dd}-to-{endDate:yyyy-MM-dd}.xlsx");
                }
                else if (section == "room")
                {
                    // Export room revenue data to Excel
                    var roomRevenueData = _dashboardServices.GetRoomRevenueExcelData(startDate.Value, endDate.Value);
                    var excelData = _excelServices.ExportRoomRevenueToExcel(roomRevenueData, startDate.Value, endDate.Value);
                    return File(
                        excelData,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        $"RoomRevenue-{startDate:yyyy-MM-dd}-to-{endDate:yyyy-MM-dd}.xlsx");
                }

                _logger.LogWarning($"Export requested for unsupported section: {section}");
                return BadRequest($"Export for section '{section}' is not supported.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting dashboard data to Excel");
                return BadRequest("An error occurred while exporting data.");
            }
        }
    }
}
