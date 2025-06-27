using System.ComponentModel.DataAnnotations;

namespace Hotel_Management.Areas.Admin.ViewModels
{
    public class ProfileVM
    {

        [Required(ErrorMessage = "Tên đăng nhập không được để trống.")]
        [Display(Name = "Tên đăng nhập")]
        public required string UserName { get; set; }

        [Required(ErrorMessage = "Email không được để trống.")]
        [EmailAddress]
        [Display(Name = "Email")]
        public required string Email { get; set; }

        [Phone(ErrorMessage = "Số điện thoại không đúng định dạng.")]
        [Display(Name = "Số điện thoại")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Mật khẩu")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự.")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("Password", ErrorMessage = "Xác nhận mật khẩu không khớp.")]
        [DataType(DataType.Password)]
        [RequiredIfPasswordProvided("Password")]
        public string? ConfirmPassword { get; set; }

        public List<ChartDataPoint> PerformanceChartData { get; set; } = new List<ChartDataPoint>();

        public class ChartDataPoint
        {
            public string? Label { get; set; }
            public decimal Value { get; set; }
        }
    }

    // Nếu nhập mật khẩu, bắt buộc phải nhập xác nhận mật khẩu
    public class RequiredIfPasswordProvidedAttribute : ValidationAttribute
    {
        private readonly string _passwordPropertyName;

        public RequiredIfPasswordProvidedAttribute(string passwordPropertyName)
        {
            _passwordPropertyName = passwordPropertyName;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var passwordProp = validationContext.ObjectType.GetProperty(_passwordPropertyName);
            if (passwordProp == null)
                return ValidationResult.Success;

            var passwordValue = passwordProp.GetValue(validationContext.ObjectInstance) as string;

            if (!string.IsNullOrEmpty(passwordValue))
            {
                if (value == null || string.IsNullOrEmpty(value as string))
                    return new ValidationResult("Xác nhận mật khẩu không được để trống khi đã nhập mật khẩu.");
            }

            return ValidationResult.Success;
        }
    }
}
