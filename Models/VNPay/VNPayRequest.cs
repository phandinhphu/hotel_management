namespace Hotel_Management.Models.VNPay
{
    public class VNPayRequest
    {
        public string? OrderType { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public string? Name { get; set; }
    }
}