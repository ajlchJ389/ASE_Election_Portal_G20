using System.ComponentModel.DataAnnotations;

namespace ASE_Election_Portal_G20.Models
{
    public class ElectionResultsViewModel
    {

        [Display(Name = "Candidate Name")]
        public string CandidateName { get; set; }

        [Display(Name = "State")]
        public string State { get; set; }
        [Display(Name = "County")]
        public string County { get; set; }
        [Display(Name = "Position")]
        public string Position { get; set; }

        [Display(Name = "Number of Votes")]
        public int NumberOfVotes { get; set; }
        
    }
}
