using System.ComponentModel.DataAnnotations;

namespace Hotel_Management.Areas.Admin.ViewModels
{
    public class HotelfacilitiesVM
    {
        public int Id { get; set; }
        public int HotelId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Tên tiện nghi không được vượt quá 100 ký tự.")]
        public required string FacilityName { get; set; }
    }
}
