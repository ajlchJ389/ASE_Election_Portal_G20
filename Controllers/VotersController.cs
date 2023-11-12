using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASE_Election_Portal_G20.Models;
using Microsoft.AspNetCore.Authorization;
using static System.Collections.Specialized.BitVector32;

namespace ASE_Election_Portal_G20.Controllers
{
    
    public class VotersController : Controller
    {
        private readonly ElectionPortalG20Context _context;

        public VotersController(ElectionPortalG20Context context)
        {
            _context = context;
        }
        [Authorize(Roles = "Admin")]
        // GET: Voters
        public async Task<IActionResult> Index()
        {
            var electionPortalG20Context = _context.Voters.Include(v => v.CountyNavigation).Include(v => v.StateNavigation).Include(v => v.User);
            return View(await electionPortalG20Context.ToListAsync());
        }


        [Authorize(Roles = "Admin")]
        // GET: Voters/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Voters == null)
            {
                return NotFound();
            }

            var voter = await _context.Voters.FindAsync(id);
            if (voter == null)
            {
                return NotFound();
            }
            ViewData["County"] = new SelectList(_context.Counties, "CountyId", "CountyName", voter.County);
            ViewData["State"] = new SelectList(_context.States, "StateId", "StateName", voter.State);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserName", voter.UserId);
            return View(voter);
        }
        [Authorize(Roles = "Admin")]
        // POST: Voters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VoterId,UserId,FirstName,LastName,Dob,State,County,Address,Email,ContactNumber,IsDeleted")] Voter voter)
        {
            if (id != voter.VoterId)
            {
                return NotFound();
            }

            
                try
                {
               
                    _context.Update(voter);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VoterExists(voter.VoterId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
           
        }


        [Authorize(Roles = "Admin")]
        // POST: Voters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {


                if (_context.Voters == null)
                {
                    return Problem("Entity set 'ElectionPortalG20Context.Voters'  is null.");
                }
                var voter = await _context.Voters.FindAsync(id);
                var user = await _context.Users.FindAsync(voter.UserId);

                if (voter != null)
                {
                    _context.Users.Remove(user);

                    _context.Voters.Remove(voter);
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Cannot delete the record as there are some references associated with it";
            }
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin")]
        private bool VoterExists(int id)
        {
          return (_context.Voters?.Any(e => e.VoterId == id)).GetValueOrDefault();
        }
        [Authorize(Roles = "Voter")]
        public async Task<IActionResult> Elections()
        {
            var Elections = new Elections_V { };
            var electionPortalG20Context = _context.Elections.Include(e => e.ElectionType).Include(e => e.Position);
            var results = await electionPortalG20Context.ToListAsync();
            var voterIdClaim = User.FindFirst("VoterId");
            int voterId = voterIdClaim != null ? int.Parse(voterIdClaim.Value) : 0;
            List<Elections_V> newResults = new List<Elections_V>();

            foreach (var result in results)
            {
                var newElection = new Elections_V
                {
                    ElectionId = result.ElectionId,
                    ElectionYear = result.ElectionYear,
                    ElectionTypeId = result.ElectionTypeId,
                    Description = result.Description,
                    StartDate = result.StartDate,
                    EndDate = result.EndDate,
                    PositionId = result.PositionId,
                    Position = result.Position,
                    ElectionType = result.ElectionType,
                    IsDeleted = result.IsDeleted,
                    HasVoted = _context.Votes.Any(v => v.ElectionId == result.ElectionId && v.VoterId == voterId)
            };

                newResults.Add(newElection);
            }

            return View(newResults);
        }
        [Authorize(Roles = "Voter")]
        [HttpGet]
        public IActionResult Vote(int id)
        {
            var voterIdClaim = User.FindFirst("VoterId");
            int voterId = voterIdClaim != null ? int.Parse(voterIdClaim.Value) : 0;
            var voter = _context.Voters.FirstOrDefault(v => v.VoterId == voterId);
            var Election = _context.Elections.Include(c => c.ElectionType).FirstOrDefault(e => e.ElectionId == id);
            ViewBag.Elections = _context.Elections.Where(c => c.ElectionId == id).ToList();
            ViewBag.Voters = _context.Voters.Where(v => v.VoterId == voterId).ToList();
            var query = _context.Candidates.Where(a => a.IsVerified && a.Approved && !a.IsRejected && a.ElectionTypeId == Election.ElectionTypeId && a.NominatedPositionId == Election.PositionId);
            if(Election.ElectionType.ElectionTypeName == "State")
            {
                query = query.Where(c => c.State == voter.State);
            }
            else if (Election.ElectionType.ElectionTypeName == "Local")
            {
                query = query.Where(c => c.County == voter.County);

            }
            ViewBag.Candidates = query.ToList();
            if(ViewBag.Candidates.Count <= 0) {
                TempData["ErrorMessage"] = "Sorry No Candidates are participating in this election from your area";

            }
            return View();
        }
        [Authorize(Roles = "Voter")]
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Vote(int electionId, int voterId, int candidateId)
        {
            var voter = new Vote
            {
                ElectionId = electionId,
                VoterId = voterId,
                CandidateId = candidateId,
                IsDeleted = false
           
            };
            _context.Add(voter);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Elections));
        }
    }
}
