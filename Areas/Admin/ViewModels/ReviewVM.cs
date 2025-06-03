using System.ComponentModel.DataAnnotations;

namespace Hotel_Management.Areas.Admin.ViewModels
{
    public class ReviewVM
    {
        public int Id { get; set; }

        [Required]
        public int? HotelId { get; set; }

        [Required]
        public string? UserId { get; set; }

        [Range(1, 5)]
        public int? Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
