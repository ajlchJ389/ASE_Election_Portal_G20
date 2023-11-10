using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ASE_Election_Portal_G20.Models;

public partial class CandidateDocument
{
    public int DocumentId { get; set; }

    [Display(Name = "Candidate")]
    public int CandidateId { get; set; }

    [Display(Name = "Document Name")]
    public string DocumentName { get; set; } = null!;

    [Display(Name = "Document URL")]
    public string DocumentUrl { get; set; } = null!;

    [Display(Name = "Is Verified?")]
    public bool IsVerified { get; set; }

    [Display(Name = "Is Rejected?")]
    public bool IsRejected { get; set; }


    public bool IsDeleted { get; set; }

    public virtual Candidate Candidate { get; set; } = null!;
}
