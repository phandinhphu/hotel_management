using System;
using System.Collections.Generic;

namespace Hotel_Management.Models;

public partial class Hotel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Address { get; set; }

    public string? Description { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public float? Rating { get; set; }

    public string? CreatedBy { get; set; }

    public virtual ApplicationUser? CreatedByNavigation { get; set; }

    public virtual ICollection<Hotelfacility> Hotelfacilities { get; set; } = new List<Hotelfacility>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();
}
