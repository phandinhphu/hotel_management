using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hotel_Management.Pages
{
    public class ContactModel : PageModel
    {
        private readonly IEmailSender _emailSender;
        private readonly ILogger<ContactModel> _logger;
        private readonly IConfiguration _configuration;

        public ContactModel(
            IEmailSender emailSender,
            ILogger<ContactModel> logger,
            IConfiguration configuration)
        {
            _emailSender = emailSender;
            _logger = logger;
            _configuration = configuration;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync(
            string username,
            string email,
            string content)
        {
            try
            {
                var emailAdmin = _configuration["EmailAdmin"];

                if (string.IsNullOrEmpty(emailAdmin))
                {
                    TempData["ErrorMessage"] = "Địa chỉ email quản trị viên chưa được cấu hình.";
                    return RedirectToPage("/Contact");
                }

                await _emailSender.SendEmailAsync(
                    emailAdmin,
                    "Tin nhắn từ người dùng gửi đến trang web quản lý khách sạn Sona",
                    $"<strong>Tin nhắn từ email {email} với tên {username}:</strong><p>{content}</p>");
                TempData["SuccessMessage"] = "Phản hồi của bạn đã được gửi thành công!";

                return RedirectToPage("/Contact");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Đã xảy ra lỗi khi gửi email";
                _logger.LogError(ex, "Lỗi khi gửi email liên hệ từ trang web.");
                return RedirectToPage("/Contact");
            }
        }
    }
}
