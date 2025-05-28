namespace Hotel_Management.Areas.Admin.ViewModels
{
    public class RoomVM
    {
        public int ID { get; set; }
        public int HotelId { get; set; }
        public string RoomNumber { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty; 
        public decimal Price { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public string Image { get; set; } = string.Empty;
        public List<IFormFile> ImageFiles { get; set; } = new List<IFormFile>();

    }
}
