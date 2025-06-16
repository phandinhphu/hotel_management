using System.Configuration;
using System.Globalization;
using System.Security.Claims;
using Hotel_Management.Areas.Admin.Services.Interfaces;
using Hotel_Management.Hubs;
using Hotel_Management.Models;
using Hotel_Management.Models.VNPay;
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
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IVNPayServices _vnpayServices;

        public BookingController(
            IBookingServices bookingServices,
            IEmailSender emailSender,
            IHubContext<NotificationHub> hubContext,
            IVNPayServices vnpayServices)
        {
            _bookingServices = bookingServices;
            _emailSender = emailSender;
            _hubContext = hubContext;
            _vnpayServices = vnpayServices;
        }

        // [GET] /Admin/Booking
        public async Task<IActionResult> Index()
        {
            // Reset TempData messages
            TempData.Clear();

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
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                _bookingServices.ApproveBooking(id, userId);

                // Tạo link thanh toán qua VNPay bằng class VNPayHelper
                var booking = await _bookingServices.GetBookingByIdAsync(id);

                var paymentUrl = _vnpayServices.CreatePaymentUrl(
                    new VNPayRequest
                    {
                        Name = booking.User.UserName,
                        Amount = booking.TotalPrice ?? 0,
                        Description = $"Thanh toán đặt phòng #{booking.BookingsRoomDetails.First().Room.RoomNumber}",
                        OrderType = "other"
                    },
                    HttpContext,
                    booking.Id);

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
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                _bookingServices.RejectBooking(id, userId);

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

        // [POST] /Admin/Booking/Delete/{id}
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var booking = await _bookingServices.GetBookingByIdAsync(id);
                if (booking == null)
                {
                    return NotFound();
                }

                var result = await _bookingServices.DeleteBookingAsync(id);

                if (result)
                {
                    TempData["SuccessMessage"] = "Đặt phòng đã được xóa thành công.";
                } else
                {
                    TempData["ErrorMessage"] = "Có lỗi xảy ra khi xóa đặt phòng. Vui lòng thử lại sau.";
                }
                
                return RedirectToAction("Index");
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
