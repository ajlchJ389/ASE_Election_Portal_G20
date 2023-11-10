using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ASE_Election_Portal_G20.Models;

public partial class State
{
    public int StateId { get; set; }

    [Display(Name = "State")]
    public string StateName { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public virtual ICollection<Candidate> Candidates { get; set; } = new List<Candidate>();

    public virtual ICollection<County> Counties { get; set; } = new List<County>();

    public virtual ICollection<Voter> Voters { get; set; } = new List<Voter>();
}
