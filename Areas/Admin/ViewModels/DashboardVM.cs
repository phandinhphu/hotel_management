using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hotel_Management.Areas.Admin.ViewModels
{
    public class DashboardVM
    {
        // Bộ lọc
        #region Filter
        [Display(Name = "Từ ngày")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Display(Name = "Đến ngày")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        #endregion

        // Tổng quan chung
        #region Summary
        public int TotalBookings { get; set; }
        public int TotalCustomers { get; set; }
        public decimal TotalRevenueService { get; set; }
        public decimal TotalRevenueRoom { get; set; }
        #endregion

        // Biểu đồ
        #region Chart
        public List<ChartDataPoint> RevenueChartData { get; set; } = new List<ChartDataPoint>();
        public List<ChartDataPoint> BookingChartData { get; set; } = new List<ChartDataPoint>();
        public List<ChartDataPoint> RoomTypeChartData { get; set; } = new List<ChartDataPoint>();

        #endregion

        // Danh sách
        #region List
        public List<ServiceRevenueItem> ServiceRevenueItems { get; set; } = new List<ServiceRevenueItem>();
        public List<RoomTypeRevenueItem> RoomTypeRevenueItems { get; set; } = new List<RoomTypeRevenueItem>();
        public List<UserReview> UserReviews { get; set; } = new List<UserReview>();
        public List<StaffInfo> StaffInfos { get; set; } = new List<StaffInfo>();
        public List<PopularRoomType> PopularRoomTypes { get; set; } = new List<PopularRoomType>();

        #endregion

        // Các lớp hỗ trợ
        #region Support Class
        public class ChartDataPoint
        {
            public string Label { get; set; }
            public decimal Value { get; set; } // Giá trị
        }

        public class ServiceRevenueItem
        {
            public string ServiceName { get; set; } // Tên dịch vụ (Taxi, Giặt ủi, v.v.)
            public decimal Revenue { get; set; } // Doanh thu
        }

        public class RoomTypeRevenueItem
        {
            public string RoomTypeName { get; set; } // Tên loại phòng
            public decimal Revenue { get; set; } // Doanh thu
        }

        public class UserReview
        {
            public string Content { get; set; } // Nội dung đánh giá
            public string RoomType { get; set; } // Loại phòng được đánh giá
        }

        public class StaffInfo
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Role { get; set; } // Quyền
            public string Status { get; set; } // Trạng thái (Online, Offline)
            public DateTime? LastActiveTime { get; set; } // Thời gian hoạt động cuối cùng
        }

        public class PopularRoomType
        {
            public string RoomNumber { get; set; } // Tên phòng
            public string ImageUrl { get; set; } // Đường dẫn ảnh
            public int BookingCount { get; set; } // Số lượt đặt
            public decimal Revenue { get; set; } // Doanh thu
        }
        #endregion

    }
}
