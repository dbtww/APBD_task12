using System;
using System.Collections.Generic;

namespace APBD_tutorial12.Models;

public partial class CarRental
{
    public int Id { get; set; }

    public int ClientId { get; set; }

    public int CarId { get; set; }

    public DateTime DateFrom { get; set; }

    public DateTime DateTo { get; set; }

    public int TotalPrice { get; set; }

    public int? Discount { get; set; }

    public virtual Car Car { get; set; } = null!;

    public virtual Client1 Client { get; set; } = null!;
}
