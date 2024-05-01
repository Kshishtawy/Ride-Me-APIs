using System;
using System.Collections.Generic;

namespace RideMe.Models;

public partial class User
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int? RoleId { get; set; }

    public int? StatusId { get; set; }

    public virtual Driver? Driver { get; set; }

    public virtual Passenger? Passenger { get; set; }

    public virtual Role? Role { get; set; }

    public virtual UserStatus? Status { get; set; }
}
