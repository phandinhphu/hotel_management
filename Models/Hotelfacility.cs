using System;
using System.Collections.Generic;

namespace Hotel_Management.Models;

public partial class Hotelfacility
{
    public int Id { get; set; }

    public int? HotelId { get; set; }

    public string? FacilityName { get; set; }

    public virtual Hotel? Hotel { get; set; }
}
