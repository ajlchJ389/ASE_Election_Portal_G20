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
    [Authorize(Roles = "Admin")]
    public class VotersController : Controller
    {
        private readonly ElectionPortalG20Context _context;

        public VotersController(ElectionPortalG20Context context)
        {
            _context = context;
        }

        // GET: Voters
        public async Task<IActionResult> Index()
        {
            var electionPortalG20Context = _context.Voters.Include(v => v.CountyNavigation).Include(v => v.StateNavigation).Include(v => v.User);
            return View(await electionPortalG20Context.ToListAsync());
        }

        

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
                if (voter != null)
                {
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

        private bool VoterExists(int id)
        {
          return (_context.Voters?.Any(e => e.VoterId == id)).GetValueOrDefault();
        }
    }
}
