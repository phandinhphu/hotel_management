using System.ComponentModel.DataAnnotations;

namespace Hotel_Management.Areas.Admin.ViewModels
{
    public class CreateUserVM
    {
        [Required]
        [Display(Name = "Tên đăng nhập")]
        public required string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public required string Email { get; set; }

        [Phone]
        [Display(Name = "Số điện thoại")]
        public string? PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public required string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Xác nhận mật khẩu không khớp.")]
        [Display(Name = "Xác nhận mật khẩu")]
        public required string ConfirmPassword { get; set; }
    }
}
