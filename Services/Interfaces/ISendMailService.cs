using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;

namespace Hotel_Management.Services.Interfaces
{
    public class MailContent
    {
        public string? To { get; set; }              // Địa chỉ gửi đến
        public string? Subject { get; set; }         // Chủ đề (tiêu đề email)
        public string? Body { get; set; }            // Nội dung (hỗ trợ HTML) của email

    }

    public interface ISendMailService : IEmailSender
    {
        Task SendMail(MailContent mailContent);
        Task SendEmailAsync(string email, string subject, string htmlMessage);
    }
}
