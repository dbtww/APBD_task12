using System;
using System.Collections.Generic;

namespace APBD_tutorial12.Models;

public partial class BookingAttraction
{
    public int BookingId { get; set; }

    public int AttractionId { get; set; }

    public int Amount { get; set; }

    public virtual Attraction Attraction { get; set; } = null!;

    public virtual Booking Booking { get; set; } = null!;
}
