using Hotel_Management.Models.VNPay;

namespace Hotel_Management.Areas.Admin.Services.Interfaces
{
    public interface IVNPayServices
    {
        string CreatePaymentUrl(VNPayRequest model, HttpContext context, int bookingId);
        VNPayResponse PaymentExecute(IQueryCollection collections);
    }
}
