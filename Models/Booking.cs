using System;
using System.Collections.Generic;

namespace Hotel_Management.Models;

public partial class Booking
{
    public int Id { get; set; }

    public string UserId { get; set; } = null!;

    public string? StaffId { get; set; } = null!;

    public decimal? TotalPriceRooms { get; set; }

    public decimal? TotalPriceServices { get; set; }

    public decimal? TotalPrice { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? Status { get; set; } = null;

    public virtual ICollection<BookingsRoomDetail> BookingsRoomDetails { get; set; } = new List<BookingsRoomDetail>();

    public virtual ICollection<BookingsServiceDetail> BookingsServiceDetails { get; set; } = new List<BookingsServiceDetail>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ApplicationUser Staff { get; set; } = null!;

    public virtual ApplicationUser User { get; set; } = null!;
}
