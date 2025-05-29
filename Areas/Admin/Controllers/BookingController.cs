using System.Configuration;
using System.Globalization;
using Hotel_Management.Areas.Admin.Services.Interfaces;
using Hotel_Management.Helpers;
using Hotel_Management.Hubs;
using Hotel_Management.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Hotel_Management.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Staff")]
    public class BookingController : Controller
    {
        private readonly IBookingServices _bookingServices;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;
        private readonly IHubContext<NotificationHub> _hubContext;

        public BookingController(
            IBookingServices bookingServices,
            IEmailSender emailSender,
            IConfiguration configuration,
            IHubContext<NotificationHub> hubContext)
        {
            _bookingServices = bookingServices;
            _emailSender = emailSender;
            _configuration = configuration;
            _hubContext = hubContext;
        }

        // [GET] /Admin/Booking
        public async Task<IActionResult> Index()
        {
            var bookings = await _bookingServices.GetBookingsAsync();

            return View(bookings);
        }

        // [GET] /Admin/Booking/Detail/{id}
        public async Task<IActionResult> Detail(int id)
        {
            try
            {
                var booking = await _bookingServices.GetBookingByIdAsync(id);

                return View(booking);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // [POST] /Admin/Booking/Approve/{id}
        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            try
            {
                _bookingServices.ApproveBooking(id);

                // Tạo link thanh toán qua VNPay bằng class VNPayHelper
                var booking = await _bookingServices.GetBookingByIdAsync(id);
                var paymentUrl = VNPayHelper.CreatePaymentUrl(HttpContext, new VNPayRequestModel
                {
                    Amount = booking.TotalPrice ?? 0,
                    Description = $"Thanh toán đặt phòng #{booking.Id}"
                }, _configuration, id);

                // Gửi email thông báo cho người dùng
                var emailContent = $"Chào {booking.User.UserName},<br/>" +
                                   $"Đặt phòng {booking.BookingsRoomDetails.First().Room.RoomNumber} của bạn đã được phê duyệt. Vui lòng thanh toán tại đây: <a href='{paymentUrl}'>Thanh toán</a>.<br/>" +
                                   "Cảm ơn bạn đã sử dụng dịch vụ của chúng tôi!";

                if (string.IsNullOrEmpty(booking.User.Email))
                {
                    return BadRequest("Email của người dùng không hợp lệ.");
                }

                await _emailSender.SendEmailAsync(booking.User.Email, "Đặt phòng đã được phê duyệt", emailContent);

                await _hubContext.Clients.Group($"Customer-{booking.User.Id}")
                    .SendAsync("ReceiveNotification", $"📥 Đặt phòng #{booking.BookingsRoomDetails.First().Room.RoomNumber} của bạn đã được phê duyệt.");

                TempData["SuccessMessage"] = "Đặt phòng đã được phê duyệt và email thông báo đã được gửi đến người dùng.";
                return RedirectToAction("Index");
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // [POST] /Admin/Booking/Reject/{id}
        [HttpPost]
        public async Task<IActionResult> Reject(int id)
        {
            try
            {
                _bookingServices.RejectBooking(id);

                // Gửi email thông báo cho người dùng
                var booking = await _bookingServices.GetBookingByIdAsync(id);
                var emailContent = $"Chào {booking.User.UserName},<br/>" +
                                   $"Rất tiếc, đặt phòng #{booking.Id} của bạn đã bị từ chối. Vui lòng liên hệ với chúng tôi để biết thêm chi tiết.<br/>" +
                                   "Cảm ơn bạn đã sử dụng dịch vụ của chúng tôi!";

                if (string.IsNullOrEmpty(booking.User.Email))
                {
                    return BadRequest("Email của người dùng không hợp lệ.");
                }

                await _emailSender.SendEmailAsync(booking.User.Email, "Đặt phòng bị từ chối", emailContent);

                await _hubContext.Clients.Group($"Customer-{booking.User.Id}")
                    .SendAsync("ReceiveNotification", $"📥 Đặt phòng #{booking.BookingsRoomDetails.First().Room.RoomNumber} của bạn đã bị từ chối.");

                TempData["SuccessMessage"] = "Đặt phòng đã bị từ chối và email thông báo đã được gửi đến người dùng.";
                return RedirectToAction("Index");
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
