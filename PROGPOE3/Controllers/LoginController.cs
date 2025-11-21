using Microsoft.AspNetCore.Mvc;
using PROGPOE3.Models;

namespace PROGPOE3.Controllers
{
    public class LoginController : Controller
    {
        // Hardcoded users
        private readonly Dictionary<string, (string Password, string Role)> _users = new()
        {
            { "lecturer@gmail.com", ("123", "Lecturer") },
            { "coordinator@gmail.com", ("123", "Coordinator") },
            { "manager@gmail.com", ("123", "Manager") }
        };

        // GET: Login
        public IActionResult Index()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (_users.TryGetValue(model.Email, out var user))
                {
                    if (user.Password == model.Password)
                    {
                        // Store role and email in TempData for now
                        TempData["Role"] = user.Role;
                        TempData["Email"] = model.Email;

                        // Redirect to Claims Index for lecturer or other roles
                        return RedirectToAction("Index", "Claims");
                    }
                }

                ViewBag.Error = "Invalid email or password.";
            }

            return View(model);
        }

        // Logout
        public IActionResult Logout()
        {
            TempData.Clear();
            return RedirectToAction("Index");
        }
    }
}
