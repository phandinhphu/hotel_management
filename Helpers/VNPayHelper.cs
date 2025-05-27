
using Hotel_Management.libs;
using Hotel_Management.Models;

namespace Hotel_Management.Helpers
{
    public class VNPayHelper
    {
        public static string CreatePaymentUrl(HttpContext context, VNPayRequestModel model, IConfiguration config)
        {
            var vnp_Url = config["VNPay:BaseUrl"];
            var vnp_Returnurl = config["VNPay:ReturnUrl"];
            var vnp_TmnCode = config["VNPay:TmnCode"];
            var vnp_HashSecret = config["VNPay:HashSecret"];

            if (string.IsNullOrEmpty(vnp_Url) || string.IsNullOrEmpty(vnp_Returnurl) ||
                string.IsNullOrEmpty(vnp_TmnCode) || string.IsNullOrEmpty(vnp_HashSecret))
            {
                throw new ArgumentException("VNPay configuration is not properly set.");
            }

            var tick = DateTime.Now.Ticks.ToString();
            var pay = new VnPayLibrary();
            pay.AddRequestData("vnp_Version", "2.1.0");
            pay.AddRequestData("vnp_Command", "pay");
            pay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            pay.AddRequestData("vnp_Amount", (model.Amount * 100).ToString());
            pay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", "VND");
            pay.AddRequestData("vnp_IpAddr", context.Connection.RemoteIpAddress?.ToString());
            pay.AddRequestData("vnp_Locale", "vn");
            pay.AddRequestData("vnp_OrderInfo", model.Description);
            pay.AddRequestData("vnp_OrderType", "other");
            pay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            pay.AddRequestData("vnp_TxnRef", tick);

            var paymentUrl = pay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
            return paymentUrl;
        }
    }
}
