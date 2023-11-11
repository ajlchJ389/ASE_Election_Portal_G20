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
        public IActionResult VoterRegisterOrLogin()
        {
            return View();
        }
        public IActionResult CandidateRegisterOrLogin()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SelectVoterOption(string Voteroption) {
            
            if(Voteroption == "Register")
            {
                return RedirectToAction("VoterRegisterPage");
            }
            else if (Voteroption == "Login")
            {
                return RedirectToAction("VoterLoginPage");
            }
            else
            {
                return RedirectToAction("VoterRegisterOrLogin");
            }
        }

        [HttpPost]
        public IActionResult SelectCandidateOption(string candoption)
        {

            if (candoption == "Register")
            {
                return RedirectToAction("CandidateRegisterPage");
            }
            else if (candoption == "Login")
            {
                return RedirectToAction("CandidateLoginPage");
            }
            else if (candoption == "Status")
            {
                return RedirectToAction("CandidateStatusPage");

            }
            else
            {
                return RedirectToAction("CandidateRegisterOrLogin");
            }
        }
        public IActionResult CandidateStatusPage()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CandidateSuccess(string FirstName,
    string LastName,
    DateTime DOB,
    string Email,
    string ContactNumber) {

            var candidate = _context.Candidates
                .Include(a => a.User) // If there's a navigation property to Users
                .SingleOrDefault(a => a.FirstName == FirstName && a.LastName == LastName && a.Dob == DOB && a.Email == Email && a.ContactNumber == ContactNumber);
            if (candidate != null)
            {
                if (candidate.Approved)
                {


                    TempData["SuccessMessage"] = "User registration successful!";
                    TempData["GeneratedUsername"] = candidate.User.Username;
                    TempData["GeneratedPassword"] = candidate.User.Password;
                    TempData["Success"] = candidate.Approved;

                    return RedirectToAction("CandidateRegistrationSuccess");
                }
                else
                {
                    TempData["SuccessMessage"] = "User registration successful!";
                    TempData["GeneratedUsername"] = "NA";
                    TempData["GeneratedPassword"] = "NA";
                    TempData["Success"] = false;

                    return RedirectToAction("CandidateRegistrationSuccess");
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Invalid User Details, Please verify.";
                return RedirectToAction("CandidateStatusPage");
            }
            

        }
        public IActionResult VoterRegisterPage()
        {
            ViewBag.States = _context.States.ToList();
            ViewBag.Counties = _context.Counties.ToList();
            return View();
        }
        public IActionResult CandidateRegisterPage()
        {
            ViewBag.States = _context.States.ToList();
            ViewBag.Counties = _context.Counties.ToList();
            ViewBag.Positions = _context.Positions.ToList();
            ViewBag.ElectionTypes = _context.ElectionTypes.ToList();
            return View();
        }
        public IActionResult CandidateLoginPage()
        {
            return View();
        }
        public IActionResult VoterLoginPage()
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
        public IActionResult VoterRegister(
    string FirstName,
    string LastName,
    DateTime DOB,
    int State,
    int County,
    string Address,
    string Email,
    string ContactNumber)
        {
            if (_context.Voters.Any(u => u.Email == Email || u.ContactNumber == ContactNumber))
            {
                TempData["ErrorMessage"] = "An account already exists with the provided email or phone number.";
                return RedirectToAction("VoterRegisterPage");
            }
            string generatedUsername = GenerateRandomUsername(LastName, ContactNumber);

            string generatedPassword = GenerateRandomPassword();

            var user = new User
            {
                Username = generatedUsername,
                Password = generatedPassword,
                UserType = "Voter", 
                IsDeleted = false,
            };

            _context.Users.Add(user);
            _context.SaveChanges();
            int userId = user.UserId;

            var voter = new Voter
            {
                UserId = userId,
                FirstName = FirstName,
                LastName = LastName,
                Dob = DOB,
                State = State,
                County = County,
                Address = Address,
                Email = Email,
                ContactNumber = ContactNumber,
                IsDeleted = false,
            };

            _context.Voters.Add(voter);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "User registration successful!";
            TempData["GeneratedUsername"] = generatedUsername;
            TempData["GeneratedPassword"] = generatedPassword;

            return RedirectToAction("RegistrationSuccess");
        }
        public IActionResult RegistrationSuccess()
        {
            ViewBag.SuccessMessage = TempData["SuccessMessage"] as string;
            ViewBag.GeneratedUsername = TempData["GeneratedUsername"] as string;
            ViewBag.GeneratedPassword = TempData["GeneratedPassword"] as string;

            return View();
        }

        public IActionResult CandidateRegistrationSuccess()
        {
            ViewBag.Success = TempData["Success"];
            ViewBag.SuccessMessage = TempData["SuccessMessage"] as string;
            ViewBag.GeneratedUsername = TempData["GeneratedUsername"] as string;
            ViewBag.GeneratedPassword = TempData["GeneratedPassword"] as string;

            return View();
        }
        private string GenerateRandomUsername(string lastName, string phoneNumber)
        {
            string truncatedLastName = lastName.Length >= 3 ? lastName.Substring(0, 4) : lastName;
            string truncatedPhoneNumber = phoneNumber.Length >= 3 ? phoneNumber.Substring(phoneNumber.Length - 5) : phoneNumber;
            return $"{truncatedLastName}{truncatedPhoneNumber}";

        }

        private string GenerateRandomPassword()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 6)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        [HttpPost]
        public IActionResult CandidateRegister(
    string FirstName,
    string LastName,
    DateTime DOB,
    int State,
    int County,
    string Address,
    string Email,
    string ContactNumber,
    string PoliticalParty,
    int ElectionTypeId,
    int NominatedPositionId)
        {
            if (_context.Candidates.Any(u => u.Email == Email || u.ContactNumber == ContactNumber))
            {
                TempData["ErrorMessage"] = "An account already exists with the provided email or phone number.";
                return RedirectToAction("CandidateRegisterPage");
            }
            string generatedUsername = GenerateRandomUsername(LastName, ContactNumber);

            string generatedPassword = GenerateRandomPassword();

            var user = new User
            {
                Username = generatedUsername,
                Password = generatedPassword,
                UserType = "Candidate",
                IsDeleted = false,
            };

            _context.Users.Add(user);
            _context.SaveChanges();
            int userId = user.UserId;

            var candidate = new Candidate
            {
                UserId = userId,
                FirstName = FirstName,
                LastName = LastName,
                Dob = DOB,
                State = State,
                County = County,
                Address = Address,
                Email = Email,
                ContactNumber = ContactNumber,
                PoliticalParty = PoliticalParty,
                ElectionTypeId = ElectionTypeId,
                NominatedPositionId = NominatedPositionId,
                IsVerified = false,
                IsRejected = false,
                Approved = false,
                IsDeleted = false,
            };

            _context.Candidates.Add(candidate);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "User registration successful!";
            TempData["GeneratedUsername"] = "NA";
            TempData["GeneratedPassword"] = "NA";
            TempData["Success"]= candidate.Approved;

            return RedirectToAction("CandidateRegistrationSuccess");
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
                return RedirectToAction("CandidateRegisterOrLogin");
            }
            else if (userType == "Voter")
            {
                // Redirect to the Voter registration or login page
                return RedirectToAction("VoterRegisterOrLogin");
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
        private Voter IsValidVoter(string username, string password)
        {
            var voter = _context.Voters
                .Include(a => a.User) // If there's a navigation property to Users
                .SingleOrDefault(a => a.User.Username == username && a.User.Password == password && a.User.UserType == "Voter");

            return voter; // Admin user exists
        }
        [HttpPost]
        public IActionResult VoterLogin(string username, string password)
        {
            try
            {
                var user = IsValidVoter(username, password);
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
                    return RedirectToAction("CandidateList", "Candidates");
                }

            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Invalid User Credentials";
            }
            TempData["ErrorMessage"] = "Invalid User Credentials";
            return RedirectToAction(nameof(VoterLoginPage));

        }
        private Candidate IsValidCandidate(string username, string password)
        {
            var candidate = _context.Candidates
                .Include(a => a.User) // If there's a navigation property to Users
                .SingleOrDefault(a => a.User.Username == username && a.User.Password == password && a.User.UserType == "Candidate" && a.Approved == true);

            return candidate; // Admin user exists
        }
        [HttpPost]
        public IActionResult CandidateLogin(string username, string password)
        {
            try
            {
                var user = IsValidCandidate(username, password);
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

                    var candidate = _context.Candidates.Include(a => a.User).FirstOrDefault(e => e.UserId == user.UserId);
                    // Redirect to the Admin Dashboard
                    return RedirectToAction("Edit", "Candidates",new {id = candidate.CandidateId});
                }

            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Invalid User Credentials";
            }
            TempData["ErrorMessage"] = "Invalid User Credentials";
            return RedirectToAction(nameof(CandidateLoginPage));

        }
    }
}
