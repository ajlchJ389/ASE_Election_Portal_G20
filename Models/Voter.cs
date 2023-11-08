using System;
using System.Collections.Generic;

namespace ASE_Election_Portal_G20.Models;

public partial class Voter
{
    public int VoterId { get; set; }

    public int UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public DateTime Dob { get; set; }

    public int State { get; set; }

    public int County { get; set; }

    public string Address { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string ContactNumber { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public virtual County CountyNavigation { get; set; } = null!;

    public virtual State StateNavigation { get; set; } = null!;

    public virtual User User { get; set; } = null!;

    public virtual ICollection<Vote> Votes { get; set; } = new List<Vote>();
}
