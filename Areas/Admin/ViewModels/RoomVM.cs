namespace Hotel_Management.Areas.Admin.ViewModels
{
    public class RoomVM
    {
        public int ID { get; set; }
        public int Title { get; set; }
        public int Price { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;


    }
}
