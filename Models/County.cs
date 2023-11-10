using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ASE_Election_Portal_G20.Models;

public partial class County
{
    public int CountyId { get; set; }

    [Display(Name = "County Name")]
    public string CountyName { get; set; } = null!;

    [Display(Name = "State")]
    public int StateId { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<Candidate> Candidates { get; set; } = new List<Candidate>();

    public virtual State State { get; set; } = null!;

    public virtual ICollection<Voter> Voters { get; set; } = new List<Voter>();
}
