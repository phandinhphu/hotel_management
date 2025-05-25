using System;
using System.Collections.Generic;

namespace Hotel_Management.Models;

public partial class BookingsRoomDetail
{
    public int Id { get; set; }

    public int BookingId { get; set; }

    public int RoomId { get; set; }

    public DateOnly? CheckIn { get; set; }

    public DateOnly? CheckOut { get; set; }

    public decimal? Price { get; set; }

    public virtual Booking Booking { get; set; } = null!;

    public virtual Room Room { get; set; } = null!;
}
