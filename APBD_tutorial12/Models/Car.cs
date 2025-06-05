using System;
using System.Collections.Generic;

namespace APBD_tutorial12.Models;

public partial class Car
{
    public int Id { get; set; }

    public string Vin { get; set; } = null!;

    public string Name { get; set; } = null!;

    public int Seats { get; set; }

    public int PricePerDay { get; set; }

    public int ModelId { get; set; }

    public int ColorId { get; set; }

    public virtual ICollection<CarRental> CarRentals { get; set; } = new List<CarRental>();

    public virtual Color Color { get; set; } = null!;

    public virtual Model Model { get; set; } = null!;
}
