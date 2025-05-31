using Hotel_Management.Areas.Admin.Services.Interfaces;
using Hotel_Management.libs;
using Hotel_Management.Models.VNPay;

namespace Hotel_Management.Areas.Admin.Services
{
    public class VNPayServices : IVNPayServices
    {
        private readonly IConfiguration _configuration;

        public VNPayServices(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreatePaymentUrl(VNPayRequest model, HttpContext context, int bookingId)
        {
            var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(_configuration["TimeZoneId"]);
            var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
            var tick = $"{bookingId}_{DateTime.Now.Ticks.ToString()}";
            var pay = new VnPayLibrary();
            var urlCallBack = _configuration["VNPay:ReturnUrl"];

            Console.WriteLine("Url CallBack: " + urlCallBack);

            pay.AddRequestData("vnp_Version", _configuration["VNPay:Version"]);
            pay.AddRequestData("vnp_Command", _configuration["VNPay:Command"]);
            pay.AddRequestData("vnp_TmnCode", _configuration["VNPay:TmnCode"]);
            pay.AddRequestData("vnp_Amount", ((int)model.Amount * 100).ToString());
            pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", _configuration["VNPay:CurrCode"]);
            pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(context));
            pay.AddRequestData("vnp_Locale", _configuration["VNPay:Locale"]);
            pay.AddRequestData("vnp_OrderInfo", $"{model.Name} {model.Description}");
            pay.AddRequestData("vnp_OrderType", model.OrderType);
            pay.AddRequestData("vnp_ReturnUrl", urlCallBack);
            pay.AddRequestData("vnp_TxnRef", tick);

            var paymentUrl =
                pay.CreateRequestUrl(_configuration["VNPay:BaseUrl"], _configuration["VNPay:HashSecret"]);

            return paymentUrl;
        }

        public VNPayResponse PaymentExecute(IQueryCollection collections)
        {
            var pay = new VnPayLibrary();
            var response = pay.GetFullResponseData(collections, _configuration["VNPay:HashSecret"]);

            return response;
        }
    }
}
