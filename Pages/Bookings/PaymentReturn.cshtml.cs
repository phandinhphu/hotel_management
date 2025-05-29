using Hotel_Management.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Hotel_Management.Pages.Bookings
{
    public class PaymentReturnModel : PageModel
    {
        private readonly HotelManagementContext _context;

        public PaymentReturnModel(HotelManagementContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var vnp_ResponseCode = Request.Query["vnp_ResponseCode"];
            var vnp_TxnRef = Request.Query["vnp_TxnRef"];
            var vnp_Amount = Request.Query["vnp_Amount"];
            var vnp_PayDate = Request.Query["vnp_PayDate"];
            var vnp_TransactionNo = Request.Query["vnp_TransactionNo"];

            if (vnp_ResponseCode == "00")
            {
                var booking = await _context.Bookings.FindAsync(int.Parse(vnp_TxnRef));
                if (booking != null)
                {
                    booking.Status = "Paid";

                    var payment = new Payment
                    {
                        BookingId = booking.Id,
                        Amount = decimal.Parse(vnp_Amount) / 100, // vì VNPay nhân 100
                        PaymentDate = DateTime.ParseExact(vnp_PayDate, "yyyyMMddHHmmss", null),
                        PaymentMethod = "VNPay",
                        Status = "Success"
                    };
                    _context.Payments.Add(payment);

                    await _context.SaveChangesAsync();
                }

                return RedirectToPage("/Bookings/Success");
            }
            else
            {
                return RedirectToPage("/Bookings/Fail");
            }
        }
    }
}
