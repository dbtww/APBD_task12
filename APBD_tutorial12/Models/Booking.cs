using System;
using System.Collections.Generic;

namespace APBD_tutorial12.Models;

public partial class Booking
{
    public int BookingId { get; set; }

    public int GuestId { get; set; }

    public int EmployeeId { get; set; }

    public DateTime Date { get; set; }

    public virtual ICollection<BookingAttraction> BookingAttractions { get; set; } = new List<BookingAttraction>();

    public virtual Employee Employee { get; set; } = null!;

    public virtual Guest Guest { get; set; } = null!;
}
