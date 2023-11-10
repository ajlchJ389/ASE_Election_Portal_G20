using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ASE_Election_Portal_G20.Models;

public partial class ElectionType
{
    public int ElectionTypeId { get; set; }

    [Display(Name = "Election Type")]
    public string ElectionTypeName { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public virtual ICollection<Candidate> Candidates { get; set; } = new List<Candidate>();

    public virtual ICollection<Election> Elections { get; set; } = new List<Election>();

    public virtual ICollection<Position> Positions { get; set; } = new List<Position>();
}
