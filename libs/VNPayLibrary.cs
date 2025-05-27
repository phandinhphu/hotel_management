using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Hotel_Management.libs
{
    public class VnPayLibrary
    {
        private SortedList<string, string> requestData = new SortedList<string, string>();

        public void AddRequestData(string key, string? value)
        {
            if (!string.IsNullOrEmpty(value))
                requestData.Add(key, value);
        }

        public string CreateRequestUrl(string baseUrl, string hashSecret)
        {
            var data = requestData
                .Select(kvp => $"{HttpUtility.UrlEncode(kvp.Key)}={HttpUtility.UrlEncode(kvp.Value)}")
                .ToArray();
            var query = string.Join("&", data);

            string signData = string.Join("&", requestData.Select(kvp => $"{kvp.Key}={kvp.Value}"));
            string sign = HmacSHA512(hashSecret, signData);

            return $"{baseUrl}?{query}&vnp_SecureHash={sign}";
        }

        private string HmacSHA512(string key, string inputData)
        {
            var hash = new HMACSHA512(Encoding.UTF8.GetBytes(key));
            var bytes = Encoding.UTF8.GetBytes(inputData);
            var hashed = hash.ComputeHash(bytes);
            return BitConverter.ToString(hashed).Replace("-", "").ToLower();
        }
    }
}