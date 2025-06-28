using Hotel_Management.Areas.Admin.Services.Interfaces;
using Hotel_Management.Models;
using Hotel_Management.Models.VNPay;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Hotel_Management.Pages.Bookings
{
    public class PaymentReturnModel : PageModel
    {
        private readonly HotelManagementContext _context;
        private readonly IVNPayServices _vnPayServices;
        private readonly IBookingServices _bookingServices;

        public PaymentReturnModel(
            HotelManagementContext context,
            IVNPayServices vNPayServices,
            IBookingServices bookingServices)
        {
            _context = context;
            _vnPayServices = vNPayServices;
            _bookingServices = bookingServices;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            VNPayResponse response = _vnPayServices.PaymentExecute(Request.Query);

            var bookingId = int.Parse(response.OrderId.Split('_')[0]);

            var booking = await _bookingServices.GetBookingByIdAsync(bookingId);

            if (booking == null)
            {
                return NotFound("Booking not found.");
            }

            booking.Status = "Paid";
            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();

            if (response.VnPayResponseCode == "00")
            {
                // Payment successful
                var model = new Payment
                {
                    BookingId = booking.Id,
                    Amount = booking.TotalPrice ?? 0,
                    PaymentDate = DateTime.UtcNow,
                    PaymentMethod = response.PaymentMethod,
                    Status = "Success",
                };
                _context.Payments.Add(model);
                await _context.SaveChangesAsync();

                TempData["Message"] = "Payment successful. Thank you for your booking!";

                return RedirectToPage("/Bookings/Success");
            }
            else
            {
                var model = new Payment
                {
                    BookingId = booking.Id,
                    Amount = booking.TotalPrice ?? 0,
                    PaymentDate = DateTime.UtcNow,
                    PaymentMethod = response.PaymentMethod,
                    Status = "Failed",
                };

                _context.Payments.Add(model);
                await _context.SaveChangesAsync();

                return RedirectToPage("/Bookings/Fail");
            }
        }
    }
}
