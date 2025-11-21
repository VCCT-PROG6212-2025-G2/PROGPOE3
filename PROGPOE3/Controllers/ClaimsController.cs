using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PROGPOE3.Data;
using PROGPOE3.Models;

namespace PROGPOE3.Controllers
{
    public class ClaimsController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly Dictionary<string, (string Password, string Role)> _users = new()
        {
            { "lecturer@gmail.com", ("123", "Lecturer") },
            { "coordinator@gmail.com", ("123", "Coordinator") },
            { "manager@gmail.com", ("123", "Manager") }
        };

        public ClaimsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Claims/Login
        [HttpGet]
        public IActionResult Login() => View();

        // POST: Claims/Login
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            if (_users.ContainsKey(email) && _users[email].Password == password)
            {
                HttpContext.Session.SetString("UserRole", _users[email].Role);
                return RedirectToAction("Index");
            }

            ViewBag.Error = "Invalid email or password";
            return View();
        }

        // GET: Claims
        public async Task<IActionResult> Index()
        {
            var claims = await _context.Claims
                .Include(c => c.ClaimStatus)
                .OrderByDescending(c => c.DateSubmitted)
                .ToListAsync();

            ViewBag.UserRole = HttpContext.Session.GetString("UserRole");
            return View(claims);
        }

        // GET: Claims/Create
        public IActionResult Create()
        {
            ViewData["ClaimStatuses"] = _context.ClaimStatuses.ToList();
            return View();
        }

        // POST: Claims/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Claim claim, IFormFile? SupportingDocument)
        {
            ViewData["ClaimStatuses"] = _context.ClaimStatuses.ToList();

            // Server-side auto-calculation
            claim.Amount = claim.HoursWorked * claim.HourlyRate;

            if (!ModelState.IsValid)
                return View(claim);

            // File upload
            if (SupportingDocument != null && SupportingDocument.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid() + "_" + SupportingDocument.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using var fileStream = new FileStream(filePath, FileMode.Create);
                await SupportingDocument.CopyToAsync(fileStream);

                claim.SupportingDocument = "/uploads/" + uniqueFileName;
            }

            claim.DateSubmitted = DateTime.Now;

            _context.Claims.Add(claim);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // Approve claim with workflow automation
        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            var claim = await _context.Claims.FindAsync(id);
            if (claim != null)
            {
                // Automated workflow checks
                if (claim.HoursWorked > 160 || claim.HourlyRate > 1000)
                {
                    TempData["Warning"] = $"Claim cannot be approved automatically. Hours Worked: {claim.HoursWorked}, Hourly Rate: {claim.HourlyRate}";
                    return RedirectToAction(nameof(Index));
                }

                // Auto-approve if within limits
                claim.ClaimStatusId = 4; // Approved
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // Reject claim
        [HttpPost]
        public async Task<IActionResult> Reject(int id)
        {
            var claim = await _context.Claims.FindAsync(id);
            if (claim != null)
            {
                claim.ClaimStatusId = 5; // Rejected
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
