using System;
using System.Collections.Generic;

namespace Hotel_Management.Models;

public partial class BookingsRoomDetail
{
    public int Id { get; set; }

    public int RoomId { get; set; }

    public DateOnly? CheckIn { get; set; }

    public DateOnly? CheckOut { get; set; }

    public decimal? Price { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual Room Room { get; set; } = null!;
}
