using ASE_Election_Portal_G20.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ASE_Election_Portal_G20.Controllers
{
    public class AccountController : Controller
    {
        private readonly ElectionPortalG20Context _context;

        public AccountController(ElectionPortalG20Context context)
        {
            _context = context;
        }

        public IActionResult UserTypeSelection()
        {
            return View();
        }
        public IActionResult AdminLoginPage()
        {
            return View();
        }
        public IActionResult Logout()
        {
            // Perform logout
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Redirect to the login page or any other page
            return RedirectToAction("UserTypeSelection", "Account");
        }

        [HttpPost]
        public IActionResult SelectUserType(string userType)
        {
            if (userType == "Admin")
            {
                // Redirect to the Admin login page
                return RedirectToAction("AdminLoginPage");
            }
            else if (userType == "Candidate")
            {
                // Redirect to the Candidate registration or login page
                return RedirectToAction("CandidateRegistrationOrLogin");
            }
            else if (userType == "Voter")
            {
                // Redirect to the Voter registration or login page
                return RedirectToAction("VoterRegistrationOrLogin");
            }
            else
            {
                // Handle invalid selection
                return RedirectToAction("UserTypeSelection");
            }
        }
        private Admin IsValidAdmin(string username, string password)
        {
                var admin = _context.Admins
                    .Include(a => a.User) // If there's a navigation property to Users
                    .SingleOrDefault(a => a.User.Username == username && a.User.Password == password && a.User.UserType=="Admin");

                return admin; // Admin user exists
        }
        [HttpPost]
        public IActionResult AdminLogin(string username, string password)
        {
            try
            {
                var user = IsValidAdmin(username, password);
                // Check username and password
                if (user != null)
                {
                    // Assign the user's role to a claim for use in authorization
                    var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.User.Username),
        new Claim(ClaimTypes.Role, user.User.UserType) // Role from the database
    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true // Set this based on your requirements
                    };

                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                    // Redirect to the Admin Dashboard
                    return RedirectToAction("Index", "Voters");
                }
          
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Invalid User Credentials";
            }
            TempData["ErrorMessage"] = "Invalid User Credentials";
            return RedirectToAction(nameof(AdminLoginPage));
            
        }

    }
}
