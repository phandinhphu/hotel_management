using System;
using System.Collections.Generic;

namespace Hotel_Management.Models;

public partial class Review
{
    public int Id { get; set; }

    public int? HotelId { get; set; }

    public string? UserId { get; set; }

    public int? Rating { get; set; }

    public string? Comment { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Hotel? Hotel { get; set; }

    public virtual ApplicationUser? User { get; set; }
}
