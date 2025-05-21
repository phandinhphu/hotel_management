using System;
using System.Collections.Generic;

namespace Hotel_Management.Models;

public partial class Roomimage
{
    public int Id { get; set; }

    public int? RoomId { get; set; }

    public string? ImageUrl { get; set; }

    public virtual Room? Room { get; set; }
}
