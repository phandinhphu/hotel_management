using System;
using System.Collections.Generic;

namespace Hotel_Management.Models;

public partial class BookingsServiceDetail
{
    public int Id { get; set; }

    public int ServiceId { get; set; }

    public decimal? Price { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual Service Service { get; set; } = null!;
}
