using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using School_Management_System.Data;
using School_Management_System.Models;

namespace School_Management_System.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public AccountController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(User user)
        {
            // Validate the incoming user data
            if(!ModelState.IsValid)
            {
                foreach(var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }
            if(ModelState.IsValid)
            {
                if(_applicationDbContext.Users.Any(user =>user.Email == user.Email))
                {
                    ModelState.AddModelError("Email", "A user with this email already exists.");
                    return View();
                }
            }

            // Hash the password before saving
            var newUser = new User()
            {
                Username = user.Username,
                Email = user.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(user.Password),
                Role = user.Role ?? "Student"
            };

            // Save the new user to the database
            _applicationDbContext.Users.Add(newUser);
            await _applicationDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Login));
        }
        
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            if(!ModelState.IsValid)
            {
                foreach(var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }

            if(string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError(string.Empty, "Email and password are required.");
                return View();
            }

            var user =  _applicationDbContext.Users.FirstOrDefault(user => user.Email == email);
            if(user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password");
                return View();
            }

            // Create claims and sign in the user
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
            };

            // Create the identity and principal
            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            // Sign in the user
            var authProperties = new AuthenticationProperties()
            {
                IsPersistent = true, // Keep the user logged in across sessions
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1) // Set the expiration time for the cookie
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity),
                authProperties
            );

            return RedirectToAction("Index", "Dashboard");
        }
        
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }
    }
}
