using System;
using System.Collections.Generic;

namespace ASE_Election_Portal_G20.Models;

public partial class Admin
{
    public int AdminId { get; set; }

    public int UserId { get; set; }

    public string AdminName { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public virtual User User { get; set; } = null!;
}
