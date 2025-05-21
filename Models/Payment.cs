using System;
using System.Collections.Generic;

namespace Hotel_Management.Models;

public partial class Payment
{
    public int Id { get; set; }

    public int? BookingId { get; set; }

    public DateTime? PaymentDate { get; set; }

    public decimal? Amount { get; set; }

    public string? PaymentMethod { get; set; }

    public string? Status { get; set; }

    public virtual Booking? Booking { get; set; }
}
