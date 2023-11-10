using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ASE_Election_Portal_G20.Models;

public partial class Vote
{
    public int VoteId { get; set; }
    [Display(Name = "Voter")]
    public int VoterId { get; set; }
    [Display(Name = "Candidate")]
    public int CandidateId { get; set; }
    [Display(Name = "Election")]
    public int ElectionId { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Candidate Candidate { get; set; } = null!;

    public virtual Election Election { get; set; } = null!;

    public virtual Voter Voter { get; set; } = null!;
}
