using System;
using System.Collections.Generic;

namespace ASE_Election_Portal_G20.Models;

public partial class Position
{
    public int PositionId { get; set; }

    public string PositionName { get; set; } = null!;

    public int ElectionTypeId { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<Candidate> Candidates { get; set; } = new List<Candidate>();

    public virtual ElectionType ElectionType { get; set; } = null!;

    public virtual ICollection<Election> Elections { get; set; } = new List<Election>();
}
