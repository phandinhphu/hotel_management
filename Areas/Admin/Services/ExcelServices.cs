using ClosedXML.Excel;
using Hotel_Management.Areas.Admin.Services.Interfaces;
using Hotel_Management.Areas.Admin.ViewModels;

namespace Hotel_Management.Areas.Admin.Services
{
    public class ExcelServices : IExcelServices
    {
        private readonly ILogger<ExcelServices> _logger;

        public ExcelServices(ILogger<ExcelServices> logger)
        {
            _logger = logger;
        }

        public byte[] ExportServiceRevenueToExcel(List<ServiceRevenueExcel> data, DateTime startDate, DateTime endDate)
        {
            try
            {
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Doanh Thu Dịch Vụ");

                    // Add title and date range
                    worksheet.Cell("A1").Value = "BÁO CÁO DOANH THU DỊCH VỤ";
                    worksheet.Cell("A2").Value = $"Từ ngày: {startDate:dd/MM/yyyy} đến ngày: {endDate:dd/MM/yyyy}";
                    worksheet.Cell("A3").Value = $"Xuất báo cáo lúc: {DateTime.Now:dd/MM/yyyy HH:mm:ss}";

                    // Format title
                    worksheet.Range("A1:I1").Merge();
                    worksheet.Range("A2:I2").Merge();
                    worksheet.Range("A3:I3").Merge();

                    worksheet.Cell("A1").Style.Font.Bold = true;
                    worksheet.Cell("A1").Style.Font.FontSize = 16;
                    worksheet.Cell("A1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                    // Add headers
                    worksheet.Cell("A5").Value = "STT";
                    worksheet.Cell("B5").Value = "Mã dịch vụ";
                    worksheet.Cell("C5").Value = "Tên dịch vụ";
                    worksheet.Cell("D5").Value = "Đơn giá";
                    worksheet.Cell("E5").Value = "Khách hàng";
                    worksheet.Cell("F5").Value = "Nhân viên";
                    worksheet.Cell("G5").Value = "Ngày đặt";
                    worksheet.Cell("H5").Value = "Trạng thái";
                    worksheet.Cell("I5").Value = "Ngày thanh toán";

                    // Format headers
                    var headerRange = worksheet.Range("A5:I5");
                    headerRange.Style.Font.Bold = true;
                    headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
                    headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    headerRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                    // Add data
                    if (data != null && data.Any())
                    {
                        int row = 6;
                        int stt = 1;
                        decimal totalRevenue = 0;

                        foreach (var item in data)
                        {
                            worksheet.Cell($"A{row}").Value = stt++;
                            worksheet.Cell($"B{row}").Value = item.ServiceId;
                            worksheet.Cell($"C{row}").Value = item.ServiceName;
                            worksheet.Cell($"D{row}").Value = item.Price;
                            worksheet.Cell($"E{row}").Value = item.CustomerName;
                            worksheet.Cell($"F{row}").Value = item.StaffName;
                            worksheet.Cell($"G{row}").Value = item.CreatedAt;
                            worksheet.Cell($"H{row}").Value = item.PaymentStatus;
                            worksheet.Cell($"I{row}").Value = item.PaymentDate;

                            // Format cells
                            worksheet.Cell($"D{row}").Style.NumberFormat.Format = "#,##0";
                            worksheet.Cell($"G{row}").Style.NumberFormat.Format = "dd/MM/yyyy";
                            worksheet.Cell($"I{row}").Style.NumberFormat.Format = "dd/MM/yyyy";

                            // Add style to the row
                            worksheet.Range($"A{row}:I{row}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            worksheet.Range($"A{row}:I{row}").Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                            // Center some columns
                            worksheet.Cell($"A{row}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            worksheet.Cell($"B{row}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            worksheet.Cell($"G{row}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            worksheet.Cell($"H{row}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            worksheet.Cell($"I{row}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                            // Add color for status
                            if (item.PaymentStatus == "Success")
                            {
                                totalRevenue += item.Price;
                                worksheet.Cell($"H{row}").Style.Font.FontColor = XLColor.Green;
                            }
                            else
                            {
                                worksheet.Cell($"H{row}").Style.Font.FontColor = XLColor.Red;
                            }

                            row++;
                        }

                        // Add total row
                        worksheet.Cell($"A{row}").Value = "TỔNG CỘNG";
                        worksheet.Range($"A{row}:H{row}").Merge();
                        worksheet.Cell($"A{row}").Style.Font.Bold = true;
                        worksheet.Cell($"A{row}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                        worksheet.Cell($"I{row}").Value = totalRevenue;
                        worksheet.Cell($"I{row}").Style.Font.Bold = true;
                        worksheet.Cell($"I{row}").Style.NumberFormat.Format = "#,##0";

                        worksheet.Range($"A{row}:I{row}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Range($"A{row}:I{row}").Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Range($"A{row}:I{row}").Style.Fill.BackgroundColor = XLColor.LightBlue;
                    }
                    else
                    {
                        worksheet.Cell("A6").Value = "Không có dữ liệu doanh thu dịch vụ trong khoảng thời gian này";
                        worksheet.Range("A6:I6").Merge();
                        worksheet.Cell("A6").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        worksheet.Range("A6:I6").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    }

                    // Auto-fit columns
                    worksheet.Columns().AdjustToContents();

                    // Save to memory stream
                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        return stream.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xuất dữ liệu doanh thu dịch vụ ra Excel");
                throw;
            }
        }

        public byte[] ExportRoomRevenueToExcel(List<RoomRevenueExcel> data, DateTime startDate, DateTime endDate)
        {
            try
            {
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Doanh Thu Phòng");

                    // Add title and date range
                    worksheet.Cell("A1").Value = "BÁO CÁO DOANH THU PHÒNG";
                    worksheet.Cell("A2").Value = $"Từ ngày: {startDate:dd/MM/yyyy} đến ngày: {endDate:dd/MM/yyyy}";
                    worksheet.Cell("A3").Value = $"Xuất báo cáo lúc: {DateTime.Now:dd/MM/yyyy HH:mm:ss}";

                    // Format title
                    worksheet.Range("A1:L1").Merge();
                    worksheet.Range("A2:L2").Merge();
                    worksheet.Range("A3:L3").Merge();

                    worksheet.Cell("A1").Style.Font.Bold = true;
                    worksheet.Cell("A1").Style.Font.FontSize = 16;
                    worksheet.Cell("A1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                    // Add headers
                    worksheet.Cell("A5").Value = "STT";
                    worksheet.Cell("B5").Value = "Mã phòng";
                    worksheet.Cell("C5").Value = "Số phòng";
                    worksheet.Cell("D5").Value = "Loại phòng";
                    worksheet.Cell("E5").Value = "Đơn giá";
                    worksheet.Cell("F5").Value = "Khách hàng";
                    worksheet.Cell("G5").Value = "Nhân viên";
                    worksheet.Cell("H5").Value = "Ngày nhận";
                    worksheet.Cell("I5").Value = "Ngày trả";
                    worksheet.Cell("J5").Value = "Ngày đặt";
                    worksheet.Cell("K5").Value = "Trạng thái";
                    worksheet.Cell("L5").Value = "Ngày thanh toán";

                    // Format headers
                    var headerRange = worksheet.Range("A5:L5");
                    headerRange.Style.Font.Bold = true;
                    headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
                    headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    headerRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                    // Add data
                    if (data != null && data.Any())
                    {
                        int row = 6;
                        int stt = 1;
                        decimal totalRevenue = 0;

                        foreach (var item in data)
                        {
                            worksheet.Cell($"A{row}").Value = stt++;
                            worksheet.Cell($"B{row}").Value = item.RoomId;
                            worksheet.Cell($"C{row}").Value = item.RoomNumber;
                            worksheet.Cell($"D{row}").Value = item.RoomType;
                            worksheet.Cell($"E{row}").Value = item.Price;
                            worksheet.Cell($"F{row}").Value = item.CustomerName;
                            worksheet.Cell($"G{row}").Value = item.StaffName;
                            worksheet.Cell($"H{row}").Value = item.CheckIn.ToString("dd/MM/yyyy");
                            worksheet.Cell($"I{row}").Value = item.CheckOut.ToString("dd/MM/yyyy");
                            worksheet.Cell($"J{row}").Value = item.CreatedAt;
                            worksheet.Cell($"K{row}").Value = item.PaymentStatus;
                            worksheet.Cell($"L{row}").Value = item.PaymentDate;

                            // Format cells
                            worksheet.Cell($"E{row}").Style.NumberFormat.Format = "#,##0";
                            worksheet.Cell($"J{row}").Style.NumberFormat.Format = "dd/MM/yyyy";
                            worksheet.Cell($"L{row}").Style.NumberFormat.Format = "dd/MM/yyyy";

                            // Add style to the row
                            worksheet.Range($"A{row}:L{row}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            worksheet.Range($"A{row}:L{row}").Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                            // Center some columns
                            worksheet.Cell($"A{row}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            worksheet.Cell($"B{row}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            worksheet.Cell($"H{row}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            worksheet.Cell($"I{row}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            worksheet.Cell($"J{row}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            worksheet.Cell($"K{row}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            worksheet.Cell($"L{row}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                            // Add color for status
                            if (item.PaymentStatus == "Success")
                            {
                                totalRevenue += item.Price;
                                worksheet.Cell($"K{row}").Style.Font.FontColor = XLColor.Green;
                            }
                            else
                            {
                                worksheet.Cell($"K{row}").Style.Font.FontColor = XLColor.Red;
                            }

                            row++;
                        }

                        // Add total row
                        worksheet.Cell($"A{row}").Value = "TỔNG CỘNG";
                        worksheet.Range($"A{row}:K{row}").Merge();
                        worksheet.Cell($"A{row}").Style.Font.Bold = true;
                        worksheet.Cell($"A{row}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                        worksheet.Cell($"L{row}").Value = totalRevenue;
                        worksheet.Cell($"L{row}").Style.Font.Bold = true;
                        worksheet.Cell($"L{row}").Style.NumberFormat.Format = "#,##0";

                        worksheet.Range($"A{row}:L{row}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Range($"A{row}:L{row}").Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Range($"A{row}:L{row}").Style.Fill.BackgroundColor = XLColor.LightBlue;
                    }
                    else
                    {
                        worksheet.Cell("A6").Value = "Không có dữ liệu doanh thu phòng trong khoảng thời gian này";
                        worksheet.Range("A6:L6").Merge();
                        worksheet.Cell("A6").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        worksheet.Range("A6:L6").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    }

                    // Auto-fit columns
                    worksheet.Columns().AdjustToContents();

                    // Save to memory stream
                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        return stream.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xuất dữ liệu doanh thu phòng ra Excel");
                throw;
            }
        }

        public byte[] ExportRevenueToExcel(List<RevenueExcel> data, DateTime startDate, DateTime endDate)
        {
            try
            {
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Doanh Thu Tổng Hợp");

                    // Add title and date range
                    worksheet.Cell("A1").Value = "BÁO CÁO DOANH THU TỔNG HỢP";
                    worksheet.Cell("A2").Value = $"Từ ngày: {startDate:dd/MM/yyyy} đến ngày: {endDate:dd/MM/yyyy}";
                    worksheet.Cell("A3").Value = $"Xuất báo cáo lúc: {DateTime.Now:dd/MM/yyyy HH:mm:ss}";

                    // Format title
                    worksheet.Range("A1:N1").Merge();
                    worksheet.Range("A2:N2").Merge();
                    worksheet.Range("A3:N3").Merge();

                    worksheet.Cell("A1").Style.Font.Bold = true;
                    worksheet.Cell("A1").Style.Font.FontSize = 16;
                    worksheet.Cell("A1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                    // Add headers
                    worksheet.Cell("A5").Value = "STT";
                    worksheet.Cell("B5").Value = "Mã đặt phòng";
                    worksheet.Cell("C5").Value = "Dịch vụ";
                    worksheet.Cell("D5").Value = "Giá dịch vụ";
                    worksheet.Cell("E5").Value = "Loại phòng";
                    worksheet.Cell("F5").Value = "Giá phòng";
                    worksheet.Cell("G5").Value = "Tổng tiền";
                    worksheet.Cell("H5").Value = "Khách hàng";
                    worksheet.Cell("I5").Value = "Nhân viên";
                    worksheet.Cell("J5").Value = "Ngày nhận";
                    worksheet.Cell("K5").Value = "Ngày trả";
                    worksheet.Cell("L5").Value = "Ngày đặt";
                    worksheet.Cell("M5").Value = "Trạng thái";
                    worksheet.Cell("N5").Value = "Ngày thanh toán";

                    // Format headers
                    var headerRange = worksheet.Range("A5:N5");
                    headerRange.Style.Font.Bold = true;
                    headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
                    headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    headerRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                    // Add data
                    if (data != null && data.Any())
                    {
                        int row = 6;
                        int stt = 1;
                        decimal totalRevenue = 0;

                        foreach (var item in data)
                        {
                            worksheet.Cell($"A{row}").Value = stt++;
                            worksheet.Cell($"B{row}").Value = item.BookingId;
                            worksheet.Cell($"C{row}").Value = item.ServiceName ?? "Không có dịch vụ";
                            worksheet.Cell($"D{row}").Value = item.ServicePrice;
                            worksheet.Cell($"E{row}").Value = item.RoomType ?? "Không có phòng";
                            worksheet.Cell($"F{row}").Value = item.RoomPrice;
                            worksheet.Cell($"G{row}").Value = item.TotalPrice;
                            worksheet.Cell($"H{row}").Value = item.CustomerName;
                            worksheet.Cell($"I{row}").Value = item.StaffName;
                            worksheet.Cell($"J{row}").Value = item.CheckIn.ToString("dd/MM/yyyy");
                            worksheet.Cell($"K{row}").Value = item.CheckOut.ToString("dd/MM/yyyy");
                            worksheet.Cell($"L{row}").Value = item.CreatedAt;
                            worksheet.Cell($"M{row}").Value = item.PaymentStatus;
                            worksheet.Cell($"N{row}").Value = item.PaymentDate;

                            // Format cells
                            worksheet.Cell($"D{row}").Style.NumberFormat.Format = "#,##0";
                            worksheet.Cell($"F{row}").Style.NumberFormat.Format = "#,##0";
                            worksheet.Cell($"G{row}").Style.NumberFormat.Format = "#,##0";
                            worksheet.Cell($"G{row}").Style.Font.Bold = true;
                            worksheet.Cell($"L{row}").Style.NumberFormat.Format = "dd/MM/yyyy";
                            worksheet.Cell($"N{row}").Style.NumberFormat.Format = "dd/MM/yyyy";

                            // Add style to the row
                            worksheet.Range($"A{row}:N{row}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            worksheet.Range($"A{row}:N{row}").Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                            // Center some columns
                            worksheet.Cell($"A{row}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            worksheet.Cell($"B{row}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            worksheet.Cell($"J{row}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            worksheet.Cell($"K{row}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            worksheet.Cell($"L{row}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            worksheet.Cell($"M{row}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            worksheet.Cell($"N{row}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                            // Add color for status
                            if (item.PaymentStatus == "Success")
                            {
                                totalRevenue += item.TotalPrice;
                                worksheet.Cell($"M{row}").Style.Font.FontColor = XLColor.Green;
                            }
                            else
                            {
                                worksheet.Cell($"M{row}").Style.Font.FontColor = XLColor.Red;
                            }

                            row++;
                        }

                        // Add total row
                        worksheet.Cell($"A{row}").Value = "TỔNG CỘNG";
                        worksheet.Range($"A{row}:M{row}").Merge();
                        worksheet.Cell($"A{row}").Style.Font.Bold = true;
                        worksheet.Cell($"A{row}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                        worksheet.Cell($"N{row}").Value = totalRevenue;
                        worksheet.Cell($"N{row}").Style.Font.Bold = true;
                        worksheet.Cell($"N{row}").Style.NumberFormat.Format = "#,##0";

                        worksheet.Range($"A{row}:N{row}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Range($"A{row}:N{row}").Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Range($"A{row}:N{row}").Style.Fill.BackgroundColor = XLColor.LightBlue;
                    }
                    else
                    {
                        worksheet.Cell("A6").Value = "Không có dữ liệu doanh thu tổng hợp trong khoảng thời gian này";
                        worksheet.Range("A6:N6").Merge();
                        worksheet.Cell("A6").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        worksheet.Range("A6:N6").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    }

                    // Auto-fit columns
                    worksheet.Columns().AdjustToContents();

                    // Save to memory stream
                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        return stream.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xuất dữ liệu doanh thu tổng hợp ra Excel");
                throw;
            }
        }


    }
}
