using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ASE_Election_Portal_G20.Models;

public partial class Candidate
{
    public int CandidateId { get; set; }

    [Display(Name = "User Id")]
    public int UserId { get; set; }

    [Display(Name = "Name")]
    public string FirstName { get; set; } = null!;
    
    [Display(Name = "Last Name")]
    public string LastName { get; set; } = null!;

    [Display(Name = "Date of Birth")]
    public DateTime Dob { get; set; }


    public int State { get; set; }

    public int County { get; set; }

    public string Address { get; set; } = null!;

    [Display(Name = "Political Party")]
    public string PoliticalParty { get; set; } = null!;

    [Display(Name = "Is Verified?")]
    public bool IsVerified { get; set; }

    [Display(Name = "Is Rejected?")]
    public bool IsRejected { get; set; }

    [Display(Name = "Is Approved?")]
    public bool Approved { get; set; }

    public string Email { get; set; } = null!;

    [Display(Name = "Phone No")]
    public string ContactNumber { get; set; } = null!;

    [Display(Name = " Position of Nomination")]
    public int NominatedPositionId { get; set; }

    [Display(Name = "Election type")]
    public int ElectionTypeId { get; set; }

    public bool IsDeleted { get; set; }
    [Display(Name = "Documents")]
    public virtual ICollection<CandidateDocument> CandidateDocuments { get; set; } = new List<CandidateDocument>();
    [Display(Name = "County")]
    public virtual County CountyNavigation { get; set; } = null!;
    [Display(Name = "Election Type")]
    public virtual ElectionType ElectionType { get; set; } = null!;
    [Display(Name = "Position of Nomination")]
    public virtual Position NominatedPosition { get; set; } = null!;
    [Display(Name = "State")]
    public virtual State StateNavigation { get; set; } = null!;

    public virtual User User { get; set; } = null!;

    public virtual ICollection<Vote> Votes { get; set; } = new List<Vote>();
}
