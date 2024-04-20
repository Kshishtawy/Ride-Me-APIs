using System;
using System.Collections.Generic;

namespace RideMe.Models;

public partial class City
{
    public int Id { get; set; }

    public string Name { get; set; }

    public virtual ICollection<Driver> Drivers { get; set; } = new List<Driver>();
}
