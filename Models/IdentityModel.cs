using Microsoft.AspNetCore.Identity;

namespace Hotel_Management.Models
{
    public class ApplicationUser : IdentityUser 
    {
        public virtual ICollection<Booking> BookingStaffs { get; set; } = new List<Booking>();

        public virtual ICollection<Booking> BookingUsers { get; set; } = new List<Booking>();

        public virtual ICollection<Hotel> Hotels { get; set; } = new List<Hotel>();

        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
