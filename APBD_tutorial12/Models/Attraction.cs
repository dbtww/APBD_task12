using System;
using System.Collections.Generic;

namespace APBD_tutorial12.Models;

public partial class Attraction
{
    public int AttractionId { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public virtual ICollection<BookingAttraction> BookingAttractions { get; set; } = new List<BookingAttraction>();
}
