using System;
using System.Collections.Generic;

namespace ASE_Election_Portal_G20.Models;

public partial class Election
{
    public int ElectionId { get; set; }

    public int ElectionYear { get; set; }

    public int ElectionTypeId { get; set; }

    public int PositionId { get; set; }

    public string Description { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ElectionType ElectionType { get; set; } = null!;

    public virtual Position Position { get; set; } = null!;

    public virtual ICollection<Vote> Votes { get; set; } = new List<Vote>();
}
