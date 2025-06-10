namespace Hotel_Management.Areas.Admin.ViewModels
{
    public class PaymentVM
    {
        public int Id { get; set; }
        public int BookingId {  get; set; }
        public string? CustomerName { get; set; }
        public string? StaffName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal RoomPrice { get; set; }
        public decimal ServicePrice { get; set; }
        public decimal Amount { get; set; }
        public string? PaymentMethod { get; set; }
        public string? Status { get; set; }
        public List<PaymentItemVM>? Items { get; set; }
    }

    public class PaymentItemVM
    {
        public string? Name { get; set; }
        public decimal Price { get; set; }
    }
}
