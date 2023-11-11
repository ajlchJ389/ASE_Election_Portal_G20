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
    
    public class CandidatesController : Controller
    {
        private readonly ElectionPortalG20Context _context;

        public CandidatesController(ElectionPortalG20Context context)
        {
            _context = context;
        }

        [Authorize(Roles = "Voter")]
        public async Task<IActionResult> CandidateList()
        {
            var electionPortalG20Context = _context.Candidates.Include(c => c.CountyNavigation).Include(c => c.ElectionType).Include(c => c.NominatedPosition).Include(c => c.StateNavigation).Include(c => c.User);
            return View(await electionPortalG20Context.ToListAsync());
        }

        [Authorize(Roles = "Admin")]

        // GET: Candidates
        public async Task<IActionResult> Index()
        {
            var electionPortalG20Context = _context.Candidates.Include(c => c.CountyNavigation).Include(c => c.ElectionType).Include(c => c.NominatedPosition).Include(c => c.StateNavigation).Include(c => c.User);
            return View(await electionPortalG20Context.ToListAsync());
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Documents(int id)
        {
            if (id != 0)
            {
                return NotFound();
            }
            return View();

        }
            [Authorize(Roles = "Admin, Candidate")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Candidates == null)
            {
                return NotFound();
            }

            var candidate = await _context.Candidates.FindAsync(id);
            if (candidate == null)
            {
                return NotFound();
            }
            ViewData["County"] = new SelectList(_context.Counties, "CountyId", "CountyName", candidate.County);
            ViewData["ElectionTypeId"] = new SelectList(_context.ElectionTypes, "ElectionTypeId", "ElectionTypeName", candidate.ElectionTypeId);
            ViewData["NominatedPositionId"] = new SelectList(_context.Positions, "PositionId", "PositionName", candidate.NominatedPositionId);
            ViewData["State"] = new SelectList(_context.States, "StateId", "StateName", candidate.State);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserName", candidate.UserId);
            return View(candidate);
        }
        [Authorize(Roles = "Admin, Candidate")]

        // POST: Candidates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CandidateId,UserId,FirstName,LastName,Dob,State,County,Address,PoliticalParty,IsVerified,IsRejected,Approved,Email,ContactNumber,NominatedPositionId,ElectionTypeId,IsDeleted")] Candidate candidate, bool approve, bool reject, bool verify)
        {
            if (id != candidate.CandidateId)
            {
                return NotFound();
            }

            
                try
                {
                
                if (approve)
                {
                    // Handle approval logic
                    candidate.IsVerified = true;
                    candidate.Approved = true;
                    candidate.IsRejected = false;
                }
                else if (reject)
                {
                    // Handle rejection logic
                    candidate.IsRejected = true;
                    candidate.IsVerified = false;
                    candidate.Approved = false;
                }
                else if (verify)
                {
                    candidate.IsVerified = true;
                }
                
                _context.Update(candidate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CandidateExists(candidate.CandidateId))
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
        // POST: Candidates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {


                if (_context.Candidates == null)
                {
                    return Problem("Entity set 'ElectionPortalG20Context.Candidates'  is null.");
                }
                var candidate = await _context.Candidates.FindAsync(id);
                if (candidate != null)
                {
                    _context.Candidates.Remove(candidate);
                }

                await _context.SaveChangesAsync();
            }
            catch(Exception)
            {
                TempData["ErrorMessage"] = "Cannot delete the record as there are some references associated with it";
            }
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin")]
        private bool CandidateExists(int id)
        {
          return (_context.Candidates?.Any(e => e.CandidateId == id)).GetValueOrDefault();
        }
    }
}
