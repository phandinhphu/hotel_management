using System.ComponentModel.DataAnnotations;

namespace Hotel_Management.Areas.Admin.ViewModels
{
    public class EditUserVM
    {
        public string? UserId { get; set; }
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
    }
}
