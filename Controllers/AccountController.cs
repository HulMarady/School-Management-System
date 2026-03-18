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
    }
}
