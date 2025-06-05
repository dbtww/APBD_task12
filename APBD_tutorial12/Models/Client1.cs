using System;
using System.Collections.Generic;

namespace APBD_tutorial12.Models;

public partial class Client1
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Address { get; set; } = null!;

    public virtual ICollection<CarRental> CarRentals { get; set; } = new List<CarRental>();
}
