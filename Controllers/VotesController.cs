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
        public async Task<IActionResult> Index()
        {
            var electionPortalG20Context = _context.Votes.Include(v => v.Candidate).Include(v => v.Election).Include(v => v.Voter);
            return View(await electionPortalG20Context.ToListAsync());
        }
    }

        
}
