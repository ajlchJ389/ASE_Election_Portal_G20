using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ASE_Election_Portal_G20.Models;

public partial class User
{
    public int UserId { get; set; }

    [Display(Name = "User Name")]
    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;
    [Display(Name = "Role")]
    public string UserType { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public virtual ICollection<Admin> Admins { get; set; } = new List<Admin>();

    public virtual ICollection<Candidate> Candidates { get; set; } = new List<Candidate>();

    public virtual ICollection<Voter> Voters { get; set; } = new List<Voter>();
}
