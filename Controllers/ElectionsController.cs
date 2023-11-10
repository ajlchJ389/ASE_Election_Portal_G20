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
    public class ElectionsController : Controller
    {
        private readonly ElectionPortalG20Context _context;

        public ElectionsController(ElectionPortalG20Context context)
        {
            _context = context;
        }

        // GET: Elections
        public async Task<IActionResult> Index()
        {
            var electionPortalG20Context = _context.Elections.Include(e => e.ElectionType).Include(e => e.Position);
            return View(await electionPortalG20Context.ToListAsync());
        }

        // GET: Elections/Create
        public IActionResult Create()
        {
            ViewData["ElectionTypeId"] = new SelectList(_context.ElectionTypes, "ElectionTypeId", "ElectionTypeName");
            ViewData["PositionId"] = new SelectList(_context.Positions, "PositionId", "PositionName");
            return View();
        }

        // POST: Elections/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ElectionId,ElectionYear,ElectionTypeId,PositionId,Description,StartDate,EndDate,IsDeleted")] Election election)
        {
            
                _context.Add(election);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
        }

        // GET: Elections/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Elections == null)
            {
                return NotFound();
            }

            var election = await _context.Elections.FindAsync(id);
            if (election == null)
            {
                return NotFound();
            }
            ViewData["ElectionTypeId"] = new SelectList(_context.ElectionTypes, "ElectionTypeId", "ElectionTypeName", election.ElectionTypeId);
            ViewData["PositionId"] = new SelectList(_context.Positions, "PositionId", "PositionName", election.PositionId);
            return View(election);
        }

        // POST: Elections/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ElectionId,ElectionYear,ElectionTypeId,PositionId,Description,StartDate,EndDate,IsDeleted")] Election election)
        {
            if (id != election.ElectionId)
            {
                return NotFound();
            }
                try
                {
                    _context.Update(election);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ElectionExists(election.ElectionId))
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

        // POST: Elections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {


                if (_context.Elections == null)
                {
                    return Problem("Entity set 'ElectionPortalG20Context.Elections'  is null.");
                }
                var election = await _context.Elections.FindAsync(id);
                if (election != null)
                {
                    _context.Elections.Remove(election);
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Cannot delete the record as there are some references associated with it";
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ElectionExists(int id)
        {
          return (_context.Elections?.Any(e => e.ElectionId == id)).GetValueOrDefault();
        }
    }
}
