using System;
using System.Collections.Generic;

namespace Hotel_Management.Models;

public partial class Service
{
    public int Id { get; set; }

    public int? HotelId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public decimal? Price { get; set; }

    public virtual ICollection<BookingsServiceDetail> BookingsServiceDetails { get; set; } = new List<BookingsServiceDetail>();

    public virtual Hotel? Hotel { get; set; }
}
