using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASE_Election_Portal_G20.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace ASE_Election_Portal_G20.Controllers
{
    
    public class CandidatesController : Controller
    {
        private readonly ElectionPortalG20Context _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CandidatesController(ElectionPortalG20Context context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;

        }

        [Authorize(Roles = "Voter")]
        public IActionResult CandidateList()
        {
            //var electionPortalG20Context = _context.Candidates.Include(c => c.CountyNavigation).Include(c => c.ElectionType).Include(c => c.NominatedPosition).Include(c => c.StateNavigation).Include(c => c.User);
            return View();
        }
        [Authorize(Roles = "Voter")]
        [HttpPost]
        public IActionResult CandidateList(string candidateFilter)
        {
            TempData["ErrorMessage"] = null;
            var query = _context.Candidates.Include(c => c.CountyNavigation).Include(c => c.ElectionType).Include(c => c.NominatedPosition).Include(c => c.StateNavigation).Include(c => c.User).Where(c => c.IsVerified && c.Approved && !c.IsRejected);
            var voterIdClaim = User.FindFirst("VoterId");
            int voterId = voterIdClaim != null ? int.Parse(voterIdClaim.Value) : 0;
            var voter_state = _context.Voters.FirstOrDefault(v => v.VoterId == voterId);

            
            if (candidateFilter == "stateCandidates")
            {             
                query = query.Where(c => c.State == voter_state.State);
            }

           
            else if (candidateFilter == "countyCandidates")
            {
                query = query.Where(c => c.County == voter_state.County);
            }

            var results = query.ToList();
            if(candidateFilter == null)
            {
                TempData["ErrorMessage"] = "Please select a filter criteria";

            }
            else if (results.Count == 0)
            {
                TempData["ErrorMessage"] = "No Candidates available, Please try a different filter.";
            }

            return View(results);
        }

    
    [Authorize(Roles = "Admin")]

        // GET: Candidates
        public async Task<IActionResult> Index()
        {
            var electionPortalG20Context = _context.Candidates.Include(c => c.CountyNavigation).Include(c => c.ElectionType).Include(c => c.NominatedPosition).Include(c => c.StateNavigation).Include(c => c.User);
            return View(await electionPortalG20Context.ToListAsync());
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Documents(int id)
        {
            var documents = await _context.CandidateDocuments.Include(d => d.Candidate).Where(c => c.CandidateId == id).ToListAsync();
            return View(documents);

        }
        [Authorize(Roles = "Voter")]
        public async Task<IActionResult> candidateDocuments(int id)
        {
            var documents = await _context.CandidateDocuments.Include(d => d.Candidate).Where(c => c.CandidateId == id && c.IsVerified).ToListAsync();
            return View(documents);

        }
        [Authorize(Roles = "Candidate")]
        public async Task<IActionResult> MyDocuments(int id)
        {
            var documents = await _context.CandidateDocuments.Include(d => d.Candidate).Where(c => c.CandidateId == id).ToListAsync();
            return View(documents);

        }

        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Candidate")]
        public async Task<IActionResult> MyProfile(int? id)
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
        [Authorize(Roles = "Candidate")]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MyProfile(int id, [Bind("CandidateId,UserId,FirstName,LastName,Dob,State,County,Address,PoliticalParty,IsVerified,IsRejected,Approved,Email,ContactNumber,NominatedPositionId,ElectionTypeId,IsDeleted")] Candidate candidate, bool approve = false, bool reject = false, bool verify = false)
        {
            if (id != candidate.CandidateId)
            {
                return NotFound();
            }


            try
            {

                
                    candidate.IsVerified = false;
               

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
           
                return RedirectToAction(nameof(MyProfile));
           
        }
        [Authorize(Roles = "Admin")]

        // POST: Candidates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CandidateId,UserId,FirstName,LastName,Dob,State,County,Address,PoliticalParty,IsVerified,IsRejected,Approved,Email,ContactNumber,NominatedPositionId,ElectionTypeId,IsDeleted")] Candidate candidate, bool approve=false, bool reject = false, bool verify = false)
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
                var user = await _context.Users.FindAsync(candidate.UserId);
                if (candidate != null)
                {
                    _context.Users.Remove(user);
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
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id)
        {
            var document = await _context.CandidateDocuments.FindAsync(id);

            try
            {


                if (_context.CandidateDocuments == null)
                {
                    return Problem("Entity set 'ElectionPortalG20Context.CandidateDocuments'  is null.");
                }
                if (document != null)
                {
                    document.IsVerified = true;
                    document.IsRejected = false;
                    _context.Update(document);
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Cannot Approve the document";
            }
            return RedirectToAction("Documents", new { id = document.CandidateId });
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int id)
        {
            var document = await _context.CandidateDocuments.FindAsync(id);

            try
            {


                if (_context.CandidateDocuments == null)
                {
                    return Problem("Entity set 'ElectionPortalG20Context.CandidateDocuments'  is null.");
                }
                if (document != null)
                {
                    document.IsVerified = false;
                    document.IsRejected = true;
                    _context.Update(document);
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Cannot Reject the document";
            }
            return RedirectToAction("Documents", new { id = document.CandidateId });
        }


        [Authorize(Roles = "Candidate")]
        [HttpPost, ActionName("DeleteDoc")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteDoc(int id)
        {

            var candidateIdClaim = User.FindFirst("CandidateId");
            int candidateId = candidateIdClaim != null ? int.Parse(candidateIdClaim.Value) : 0;
            try
            {


                if (_context.CandidateDocuments == null)
                {
                    return Problem("Entity set 'ElectionPortalG20Context.CandidateDocuments'  is null.");
                }
                var document = await _context.CandidateDocuments.FindAsync(id);


                    if (document != null)
                {
                        var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", document.DocumentName);

                        // Check if the file exists before attempting to delete
                        if (System.IO.File.Exists(filePath))
                        {
                            // Delete the file
                            System.IO.File.Delete(filePath);
                        }
                        _context.CandidateDocuments.Remove(document);
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Cannot delete the record as there are some references associated with it";
            }
            return RedirectToAction("MyDocuments", new { id = candidateId });
        }
        [Authorize(Roles = "Candidate")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UploadDocument(IFormFile uploadedFile)
        {
            var candidateIdClaim = User.FindFirst("CandidateId");
            int candidateId = candidateIdClaim != null ? int.Parse(candidateIdClaim.Value) : 0;
            if (uploadedFile != null && uploadedFile.Length > 0)
            {
                // Save the file to wwwroot/uploads folder
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + uploadedFile.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    uploadedFile.CopyTo(stream);
                }
                
                // Save the file name to the CandidateDocument table
                var document = new CandidateDocument
                {
                    DocumentName = uniqueFileName,
                    DocumentUrl = uploadsFolder,
                    IsVerified = false,
                    IsRejected = false,
                    IsDeleted = false,
                    CandidateId = candidateId

                };

                _context.CandidateDocuments.Add(document);
                _context.SaveChanges();
            }

            return RedirectToAction("MyDocuments", new { id = candidateId });
        }
        [Authorize(Roles = "Candidate, Admin")]
        public IActionResult DownloadDocument(int id)
        {
            var document = _context.CandidateDocuments.Find(id);

            if (document != null)
            {
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", document.DocumentName);

                if (System.IO.File.Exists(filePath))
                {
                    var fileContent = System.IO.File.ReadAllBytes(filePath);
                    return File(fileContent, "application/octet-stream", document.DocumentName);
                }
            }

            return NotFound();
        }

    }
}
