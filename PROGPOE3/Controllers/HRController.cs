using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PROGPOE3.Data;
using PROGPOE3.Models;

namespace PROGPOE3.Controllers
{
    public class HRController : Controller
    {
        private readonly ApplicationDbContext _context;
        // Constructor
        public HRController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: HR/ApprovedClaimsReport
        // REPORT: Approved Claims
        public async Task<IActionResult> ApprovedClaimsReport()
        {
            var approvedClaims = await _context.Claims
                .Include(c => c.ClaimStatus)
                .Include(c => c.Lecturer)
                .Where(c => c.ClaimStatusId == 4)
                .OrderByDescending(c => c.DateSubmitted)
                .ToListAsync();

            return View(approvedClaims);
        }
        // GET: HR/Lecturers
        // LIST LECTURERS
        public async Task<IActionResult> Lecturers()
        {
            var lecturers = await _context.Lecturers.ToListAsync();
            return View(lecturers);
        }

        // EDIT LECTURER (GET)
        public async Task<IActionResult> EditLecturer(int id)
        {
            var lecturer = await _context.Lecturers.FindAsync(id);
            if (lecturer == null) return NotFound();

            return View(lecturer);
        }

        // EDIT LECTURER (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditLecturer(Lecturer lecturer)
        {
            if (!ModelState.IsValid)
                return View(lecturer);

            _context.Update(lecturer);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Lecturer updated successfully!";
            return RedirectToAction(nameof(Lecturers));
        }
    }
}
