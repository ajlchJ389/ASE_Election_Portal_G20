using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ASE_Election_Portal_G20.Models;

public partial class Election
{
    public int ElectionId { get; set; }

    [Display(Name = "Year")]
    public int ElectionYear { get; set; }
    [Display(Name = "Election Type")]
    public int ElectionTypeId { get; set; }
    [Display(Name = "Position")]
    public int PositionId { get; set; }

    public string Description { get; set; } = null!;
    [Display(Name = "Start Date")]
    public DateTime StartDate { get; set; }
    [Display(Name = "End Date")]
    public DateTime EndDate { get; set; }

    public bool IsDeleted { get; set; }
    [Display(Name = "Election Type")]
    public virtual ElectionType ElectionType { get; set; } = null!;

    public virtual Position Position { get; set; } = null!;

    public virtual ICollection<Vote> Votes { get; set; } = new List<Vote>();
}
