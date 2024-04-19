using System;
using System.Collections.Generic;

namespace RideMe.Models;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int? RoleId { get; set; }

    public int? StatusId { get; set; }

    public virtual ICollection<Driver> Drivers { get; set; } = new List<Driver>();

    public virtual ICollection<Passenger> Passengers { get; set; } = new List<Passenger>();

    public virtual Role? Role { get; set; }

    public virtual UserStatus? Status { get; set; }
}
