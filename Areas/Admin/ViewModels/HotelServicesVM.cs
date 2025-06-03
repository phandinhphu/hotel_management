using System.ComponentModel.DataAnnotations;

namespace Hotel_Management.Areas.Admin.ViewModels
{
    public class HotelServicesVM
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Hotel")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a hotel.")]
        public int? HotelId { get; set; }

        [Required]
        [Display(Name = "Service Name")]
        [StringLength(100, ErrorMessage = "Service name cannot exceed 100 characters.")]
        public string? Name { get; set; }

        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Required]
        [Display(Name = "Price")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
        public decimal? Price { get; set; }
    }
}
