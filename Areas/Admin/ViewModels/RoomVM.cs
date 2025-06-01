using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Hotel_Management.Areas.Admin.ViewModels
{
    public class RoomVM
    {
        public int ID { get; set; }
        public int HotelId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số phòng")]
        [StringLength(10, MinimumLength = 1, ErrorMessage = "Số phòng phải từ 1-10 ký tự")]
        [RegularExpression(@"^[A-Za-z0-9\-\.]+$", ErrorMessage = "Số phòng chỉ được chứa chữ cái, số, dấu gạch ngang và dấu chấm")]
        [Display(Name = "Số phòng")]
        public string RoomNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng chọn loại phòng")]
        [Display(Name = "Loại phòng")]
        public string Type { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập giá phòng")]
        [Range(0, 100000000, ErrorMessage = "Giá phòng phải nằm trong khoảng từ 0 đến 100.000.000 VNĐ")]
        [DataType(DataType.Currency)]
        [Display(Name = "Giá phòng (VNĐ)")]
        public decimal Price { get; set; }

        [StringLength(1000, ErrorMessage = "Mô tả không được vượt quá 1000 ký tự")]
        [Display(Name = "Mô tả phòng")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng chọn trạng thái phòng")]
        [Display(Name = "Trạng thái")]
        public string Status { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập sức chứa phòng")]
        [Range(1, 20, ErrorMessage = "Sức chứa phòng phải từ 1-20 người")]
        [Display(Name = "Sức chứa")]
        public int Capacity { get; set; }

        [Display(Name = "Ảnh phòng")]
        public string Image { get; set; } = string.Empty;

        [Display(Name = "Tải lên ảnh chính")]
        public IFormFile? ImagetFile { get; set; } = null!;

        [Display(Name = "Tải lên ảnh bổ sung")]
        public List<IFormFile> ImageFiles { get; set; } = new List<IFormFile>();

    }
}
