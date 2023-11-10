using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASE_Election_Portal_G20.Models;

namespace ASE_Election_Portal_G20.Controllers
{
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
            ViewBag.Elections = _context.Elections.ToList();
            ViewBag.States = _context.States.ToList();

            return View();
        }

        [HttpPost]
        public IActionResult ViewResults(int electionId, int stateId)
        {
            var results = _context.Votes
                .Include(v => v.Candidate)
                .Where(v => v.ElectionId == electionId && v.Candidate.State == stateId)
                .GroupBy(v => v.Candidate)
                .Select(group => new ElectionResultsViewModel
                {
                    CandidateName = $"{group.Key.FirstName} {group.Key.LastName}",
                    NumberOfVotes = group.Count()
                })
                .OrderByDescending(result => result.NumberOfVotes)
                .ToList();

            return View(results);
        }
    }

        
}
