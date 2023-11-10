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
    public class CountiesController : Controller
    {
        private readonly ElectionPortalG20Context _context;

        public CountiesController(ElectionPortalG20Context context)
        {
            _context = context;
        }

        // GET: Counties
        public async Task<IActionResult> Index()
        {
            var electionPortalG20Context = _context.Counties.Include(c => c.State);
            return View(await electionPortalG20Context.ToListAsync());
        }

        // GET: Counties/Create
        public IActionResult Create()
        {
            ViewData["StateId"] = new SelectList(_context.States, "StateId", "StateName");
            return View();
        }

        // POST: Counties/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CountyId,CountyName,StateId,IsDeleted")] County county)
        {
       
                _context.Add(county);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
        }

        // GET: Counties/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Counties == null)
            {
                return NotFound();
            }

            var county = await _context.Counties.FindAsync(id);
            if (county == null)
            {
                return NotFound();
            }
            ViewData["StateId"] = new SelectList(_context.States, "StateId", "StateName", county.StateId);
            return View(county);
        }

        // POST: Counties/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CountyId,CountyName,StateId,IsDeleted")] County county)
        {
            if (id != county.CountyId)
            {
                return NotFound();
            }

          
                try
                {
                    _context.Update(county);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CountyExists(county.CountyId))
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
        // POST: Counties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {


                if (_context.Counties == null)
                {
                    return Problem("Entity set 'ElectionPortalG20Context.Counties'  is null.");
                }
                var county = await _context.Counties.FindAsync(id);
                if (county != null)
                {
                    _context.Counties.Remove(county);
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Cannot delete the record as there are some references associated with it";
            }
            return RedirectToAction(nameof(Index));
        }

        private bool CountyExists(int id)
        {
          return (_context.Counties?.Any(e => e.CountyId == id)).GetValueOrDefault();
        }
    }
}
