namespace Hotel_Management.Models
{
    // Model temp BookingItem like CardItem
    public class BookingItem
    {
        public int RoomId { get; set; }
        public string? RoomNumber { get; set; }
        public string? RoomType { get; set; }
        public string? RoomImage { get; set; }
        public decimal Price { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public List<int> ServicesSelected { get; set; } = new List<int>();
    }
}
