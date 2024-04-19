using System;
using System.Collections.Generic;

namespace RideMe.Models;

public partial class Passenger
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public virtual ICollection<Ride> Rides { get; set; } = new List<Ride>();

    public virtual User? User { get; set; }
}
