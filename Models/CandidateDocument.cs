using System;
using System.Collections.Generic;

namespace ASE_Election_Portal_G20.Models;

public partial class CandidateDocument
{
    public int DocumentId { get; set; }

    public int CandidateId { get; set; }

    public string DocumentName { get; set; } = null!;

    public string DocumentUrl { get; set; } = null!;

    public bool IsVerified { get; set; }

    public bool IsRejected { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Candidate Candidate { get; set; } = null!;
}
