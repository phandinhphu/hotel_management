namespace Hotel_Management.Areas.Admin.ViewModels
{
    public class BookingVM
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? RoomNumber { get; set; }
        public List<int> Services { get; set; } = new List<int>();
        public DateOnly CheckInDate { get; set; }
        public DateOnly CheckOutDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string? Status { get; set; }
        public string? CreatedAt { get; set; }
    }
}
