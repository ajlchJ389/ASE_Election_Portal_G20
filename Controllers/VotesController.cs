using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASE_Election_Portal_G20.Models;
using Microsoft.AspNetCore.Authorization;

namespace ASE_Election_Portal_G20.Controllers
{
    [Authorize(Roles = "Admin, Voter, Candidate")]
    public class VotesController : Controller
    {
        private readonly ElectionPortalG20Context _context;

        public VotesController(ElectionPortalG20Context context)
        {
            _context = context;
        }

        // GET: Votes
        public IActionResult Index()
        {
            ViewBag.Elections = _context.Elections.Include(e => e.ElectionType).ToList();
            ViewBag.States = _context.States.ToList();
            ViewBag.Counties = _context.Counties.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Index(int electionId, int? stateId, int? countyId)
        {
            TempData["ErrorMessage"] = null;
            ViewBag.Elections = _context.Elections.Include(e => e.ElectionType).ToList();
            ViewBag.States = _context.States.ToList();
            ViewBag.Counties = _context.Counties.ToList();
            DateTime currentDate = DateTime.Now;
             
            var query = _context.Votes
                .Include(v => v.Candidate)
                .Where(v => v.ElectionId == electionId &&
                            _context.Elections.Any(e => e.ElectionId == v.ElectionId && e.EndDate < currentDate));

            if (stateId.HasValue)
            {
                query = query.Where(v => v.Candidate.State == stateId);
            }

            if (countyId.HasValue)
            {
                query = query.Where(v => v.Candidate.County == countyId);
            }

            var results = query
                .GroupBy(v => v.Candidate)
                .Select(group => new ElectionResultsViewModel
                {
                    CandidateName = $"{group.Key.FirstName} {group.Key.LastName}",
                    State = group.Key.StateNavigation.StateName,
                    County = group.Key.CountyNavigation.CountyName,
                    Position = group.Key.NominatedPosition.PositionName,
                    NumberOfVotes = group.Count()
                })
                .OrderByDescending(result => result.NumberOfVotes)
                .ToList();
            if(electionId == 0)
            {
                TempData["ErrorMessage"] = "Please select an election before proceeding.";

            }
            else if (results.Count == 0)
            {
                TempData["ErrorMessage"] = "No Results available as the election is either live or has not yet started.";
            }

            return View(results);
        }

    }


}
