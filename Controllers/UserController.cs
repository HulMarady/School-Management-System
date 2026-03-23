using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School_Management_System.Data;
using School_Management_System.Models;
using X.PagedList.Extensions;

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

            var users = query .OrderBy(user => user.Username)
                            .ToPagedList(page, pageSize);

            return View(users);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(User user, string ConfirmPassword)
        {
            if(ModelState.IsValid)
            {
                if(await _applicationDbContext.Users.AnyAsync(u => u.Email == user.Email))
                {
                    ModelState.AddModelError("Email", "Email already exists.");
                    return View(user);
                }

                if(user.Password != ConfirmPassword)
                {
                    ModelState.AddModelError("ConfirmPassword", "Passwords do not match");
                    return View(user);
                }

                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                user.Role = user.Role ?? "User";
                user.CreatedAt = DateTime.UtcNow;
                user.UpdatedAt = DateTime.UtcNow;

                _applicationDbContext.Users.Add(user);
                await _applicationDbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(user);
        }

        public async Task<IActionResult> Details(int id)
        {
            if(id <= 0)
               return NotFound();
            
            var user = _applicationDbContext.Users
                            .FirstOrDefault(user => user.Id == id);
            if(user == null)
                return NotFound();

            return View(user);
        }

        public async Task<IActionResult> Edit(int id)
        {
            if(id <= 0)
                return NotFound();

            var user = _applicationDbContext.Users
                            .FirstOrDefault(user => user.Id == id);

            if(user == null)
                return NotFound();
            
            return View(user);
        }
        
        [HttpPost]
        public async Task<IActionResult> Edit(int id, User user)
        {
            if(id != user.Id)
                return NotFound();
            
            if(ModelState.IsValid)
            {
                var existingUser = _applicationDbContext.Users
                                        .FirstOrDefault(user => user.Id == id);
                if(existingUser == null)
                    return NotFound();

                existingUser.Username = user.Username;
                existingUser.Email = user.Email;
                existingUser.Role = user.Role;

                _applicationDbContext.Users.Update(existingUser);
                await _applicationDbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(user);
        }

        public async Task<IActionResult> Delete(int id)
        {
            if(id <= 0)
                return NotFound();
            
            var user = _applicationDbContext.Users
                            .FirstOrDefault(user => user.Id == id);
            if(user == null)
                return NotFound();

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if(id <= 0)
                return NotFound();

            var user = _applicationDbContext.Users
                            .FirstOrDefault(user => user.Id == id);

            if(user == null)
                return NotFound();

            _applicationDbContext.Users.Remove(user);
            await _applicationDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
