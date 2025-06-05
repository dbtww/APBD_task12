using System;
using System.Collections.Generic;

namespace APBD_tutorial12.Models;

public partial class Guest
{
    public int GuestId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public DateTime DateOfBirth { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
