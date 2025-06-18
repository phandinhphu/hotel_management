namespace Hotel_Management.Areas.Admin.ViewModels
{
    public class ExcelVM
    {
        public string? Title { get; set; }
        public DateTime GeneratedDate { get; set; } = DateTime.Now;
        public string? GeneratedBy { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public List<ServiceRevenueExcel> ServiceRevenues { get; set; } = new List<ServiceRevenueExcel>();
        public List<RoomRevenueExcel> RoomRevenue { get; set; } = new List<RoomRevenueExcel>();
        public List<RevenueExcel> Revenue { get; set; } = new List<RevenueExcel>();
    }

    public class ServiceRevenueExcel
    {
        public int Id { get; set; } // Mã đặt dịch vụ
        public int ServiceId { get; set; } // Mã dịch vụ
        public int BookingId { get; set; } // Mã đặt phòng
        public string? ServiceName { get; set; } // Tên dịch vụ
        public decimal Price { get; set; } // Giá dịch vụ
        public string? CustomerName { get; set; } // Tên khách hàng
        public string? StaffName { get; set; } // Tên nhân viên phục vụ
        public DateTime CreatedAt { get; set; } // Ngày đặt
        public string? PaymentStatus { get; set; } // Trạng thái thanh toán
        public DateTime? PaymentDate { get; set; } // Ngày thanh toán

    }

    public class RoomRevenueExcel
    {
        public int Id { get; set; } // Mã đặt phòng
        public int RoomId { get; set; } // Mã phòng
        public int BookingId { get; set; } // Mã đặt phòng
        public string? RoomNumber { get; set; } // Số phòng
        public string? RoomType { get; set; } // Loại phòng
        public decimal Price { get; set; } // Giá phòng
        public string? CustomerName { get; set; } // Tên khách hàng
        public string? StaffName { get; set; } // Tên nhân viên phục vụ
        public DateOnly CheckIn { get; set; } // Ngày nhận phòng
        public DateOnly CheckOut { get; set; } // Ngày trả phòng
        public DateTime CreatedAt { get; set; } // Ngày đặt
        public string? PaymentStatus { get; set; } // Trạng thái thanh toán
        public DateTime? PaymentDate { get; set; } // Ngày thanh toán
    }

    public class RevenueExcel
    {
        public int BookingId { get; set; } // Mã đặt phòng
        public string? ServiceName { get; set; } // Tên dịch vụ
        public decimal ServicePrice { get; set; } // Giá dịch vụ
        public string? RoomType { get; set; } // Loại phòng
        public decimal RoomPrice { get; set; } // Giá phòng
        public decimal TotalPrice { get; set; } // Tổng giá
        public string? CustomerName { get; set; } // Tên khách hàng
        public string? StaffName { get; set; } // Tên nhân viên phục vụ
        public DateOnly CheckIn { get; set; } // Ngày nhận phòng
        public DateOnly CheckOut { get; set; } // Ngày trả phòng
        public DateTime CreatedAt { get; set; } // Ngày đặt
        public string? PaymentStatus { get; set; } // Trạng thái thanh toán
        public DateTime? PaymentDate { get; set; } // Ngày thanh toán
    }
}
