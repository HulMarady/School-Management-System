using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School_Management_System.Data;
using API.PagedList;
using Microsoft.EntityFrameworkCore;
using School_Management_System.Models;

namespace School_Management_System.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public UserController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<IActionResult> Index(string? search, int page = 1, int pageSize = 10)
        {
            var query = _applicationDbContext.Users.AsQueryable();

            if(!string.IsNullOrEmpty(search))
            {
                query = query.Where(user => 
                    user.Username.Contains(search) ||
                    user.Email.Contains(search) || 
                    user.Role.Contains(search)
                );
            }

            var users = await query
                            .OrderBy(user => user.Username)
                            .ToPagedListAsync(page, pageSize);

            return View(users);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            if(ModelState.IsValid)
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                user.Role = user.Role ?? "User";

                _applicationDbContext.Users.Add(user);
                await _applicationDbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(user);
        }
    }
}
