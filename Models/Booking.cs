using System;
using System.Collections.Generic;

namespace Hotel_Management.Models;

public partial class Booking
{
    public int Id { get; set; }

    public string UserId { get; set; } = null!;

    public string StaffId { get; set; } = null!;

    public int BookingRooms { get; set; }

    public int BookingServices { get; set; }

    public decimal? TotalPriceRooms { get; set; }

    public decimal? TotalPriceServices { get; set; }

    public decimal? TotalPrice { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual BookingsRoomDetail BookingRoomsNavigation { get; set; } = null!;

    public virtual BookingsServiceDetail BookingServicesNavigation { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ApplicationUser Staff { get; set; } = null!;

    public virtual ApplicationUser User { get; set; } = null!;
}
