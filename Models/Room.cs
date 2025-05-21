using System;
using System.Collections.Generic;

namespace Hotel_Management.Models;

public partial class Room
{
    public int Id { get; set; }

    public int? HotelId { get; set; }

    public string? RoomNumber { get; set; }

    public string? Type { get; set; }

    public string? Description { get; set; }

    public decimal? Price { get; set; }

    public string? Status { get; set; }

    public int? Capacity { get; set; }

    public string? Image { get; set; }

    public virtual ICollection<BookingsRoomDetail> BookingsRoomDetails { get; set; } = new List<BookingsRoomDetail>();

    public virtual Hotel? Hotel { get; set; }

    public virtual ICollection<Roomimage> Roomimages { get; set; } = new List<Roomimage>();
}
