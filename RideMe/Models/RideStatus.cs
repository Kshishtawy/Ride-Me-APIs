using System;
using System.Collections.Generic;

namespace RideMe.Models;

public partial class RideStatus
{
    public int Id { get; set; }

    public string Name { get; set; }

    public virtual ICollection<Ride> Rides { get; set; } = new List<Ride>();
}
