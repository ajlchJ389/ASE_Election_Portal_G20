using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ASE_Election_Portal_G20.Models;

public partial class Voter
{
    public int VoterId { get; set; }
    [Display(Name = "User")]
    public int UserId { get; set; }

    [Display(Name = "Name")]
    public string FirstName { get; set; } = null!;
    [Display(Name = "Last Name")]
    public string LastName { get; set; } = null!;
    [Display(Name = "Date of Birth")]
    public DateTime Dob { get; set; }

    public int State { get; set; }

    public int County { get; set; }

    public string Address { get; set; } = null!;

    public string Email { get; set; } = null!;
    [Display(Name = "Phone No")]
    public string ContactNumber { get; set; } = null!;

    public bool IsDeleted { get; set; }
    [Display(Name = "County")]
    public virtual County CountyNavigation { get; set; } = null!;
    [Display(Name = "State")]
    public virtual State StateNavigation { get; set; } = null!;

    public virtual User User { get; set; } = null!;

    public virtual ICollection<Vote> Votes { get; set; } = new List<Vote>();
}
