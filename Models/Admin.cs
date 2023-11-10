using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ASE_Election_Portal_G20.Models;

public partial class Admin
{
    public int AdminId { get; set; }

    [Display(Name = "User Id")]
    public int UserId { get; set; }

    [Display(Name = "Admin Name")]
    public string AdminName { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public virtual User User { get; set; } = null!;
}
