using System;
using System.Collections.Generic;

namespace APBD_tutorial12.Models;

public partial class Model
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Car> Cars { get; set; } = new List<Car>();
}
