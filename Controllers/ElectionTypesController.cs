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
    public class ElectionTypesController : Controller
    {
        private readonly ElectionPortalG20Context _context;

        public ElectionTypesController(ElectionPortalG20Context context)
        {
            _context = context;
        }

        // GET: ElectionTypes
        public async Task<IActionResult> Index()
        {
              return _context.ElectionTypes != null ? 
                          View(await _context.ElectionTypes.ToListAsync()) :
                          Problem("Entity set 'ElectionPortalG20Context.ElectionTypes'  is null.");
        }

        // GET: ElectionTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ElectionTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ElectionTypeId,ElectionTypeName,IsDeleted")] ElectionType electionType)
        {
                _context.Add(electionType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
        }

        // GET: ElectionTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ElectionTypes == null)
            {
                return NotFound();
            }

            var electionType = await _context.ElectionTypes.FindAsync(id);
            if (electionType == null)
            {
                return NotFound();
            }
            return View(electionType);
        }

        // POST: ElectionTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ElectionTypeId,ElectionTypeName,IsDeleted")] ElectionType electionType)
        {
            if (id != electionType.ElectionTypeId)
            {
                return NotFound();
            }

                try
                {
                    _context.Update(electionType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ElectionTypeExists(electionType.ElectionTypeId))
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

        // GET: ElectionTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ElectionTypes == null)
            {
                return NotFound();
            }

            var electionType = await _context.ElectionTypes
                .FirstOrDefaultAsync(m => m.ElectionTypeId == id);
            if (electionType == null)
            {
                return NotFound();
            }

            return View(electionType);
        }

        // POST: ElectionTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {


                if (_context.ElectionTypes == null)
                {
                    return Problem("Entity set 'ElectionPortalG20Context.ElectionTypes'  is null.");
                }
                var electionType = await _context.ElectionTypes.FindAsync(id);
                if (electionType != null)
                {
                    _context.ElectionTypes.Remove(electionType);
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Cannot delete the record as there are some references associated with it";
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ElectionTypeExists(int id)
        {
          return (_context.ElectionTypes?.Any(e => e.ElectionTypeId == id)).GetValueOrDefault();
        }
    }
}
