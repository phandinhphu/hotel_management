using System.ComponentModel.DataAnnotations;
using Hotel_Management.Models;

namespace Hotel_Management.Areas.Admin.ViewModels
{
    public class PasswordVM
    {
        public required string UserId { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống.")]
        [DataType(DataType.Password, ErrorMessage = "Định dạng mật khẩu hiện tại không hợp lệ.")]
        public required string NewPassword { get; set; } = string.Empty;

        [DataType(DataType.Password, ErrorMessage = "Định dạng mật khẩu mới không hợp lệ.")]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu mới và xác nhận mật khẩu không khớp.")]
        public required string? ConfirmPassword { get; set; }
    }
}
