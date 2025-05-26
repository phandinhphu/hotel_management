using System;
using System.Collections.Generic;

namespace Hotel_Management.Models;

public partial class BookingsServiceDetail
{
    public int Id { get; set; }

    public int BookingId { get; set; }

    public int ServiceId { get; set; }

    public decimal? Price { get; set; }

    public virtual Booking Booking { get; set; } = null!;

    public virtual Service Service { get; set; } = null!;
}
