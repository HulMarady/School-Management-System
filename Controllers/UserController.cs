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
                    user.FirstName.Contains(search) ||
                    user.LastName.Contains(search) ||
                    user.Username.Contains(search) ||
                    user.Email.Contains(search)  
                );
            }

            var users = query.OrderBy(user => user.CreatedAt)
                            .ToPagedList(page, pageSize);

            return View(users);
        }

        public async Task<IActionResult> Create()
        {

            var role = await _applicationDbContext.Roles
                                .OrderBy(role => role.Name)
                                .ToListAsync();

            ViewBag.Roles = role;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(User user, string ConfirmPassword, int RoleId)
        {
            if(ModelState.IsValid)
            {
                if(await _applicationDbContext.Users.AnyAsync(u => u.Email == user.Email))
                {
                    ModelState.AddModelError("Email", "Email already exists.");
                    var roles = await _applicationDbContext.Roles
                                        .OrderBy(role => role.Name)
                                        .ToListAsync();
                    ViewBag.Roles = roles;
                    return View(user);
                }

                if(user.Password != ConfirmPassword)
                {
                    ModelState.AddModelError("ConfirmPassword", "Passwords do not match");
                    var roles = await _applicationDbContext.Roles
                                        .OrderBy(role => role.Name)
                                        .ToListAsync();
                    ViewBag.Roles = roles;
                    return View(user);
                }

                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                user.CreatedAt = DateTime.UtcNow;
                user.UpdatedAt = DateTime.UtcNow;

                if(RoleId > 0)
                {
                    var userRole = new UserRole
                    {
                        RoleId = RoleId,
                        User = user
                    };
                    _applicationDbContext.UserRoles.Add(userRole);
                }


                _applicationDbContext.Users.Add(user);
                await _applicationDbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var allRoles = await _applicationDbContext.Roles
                                .OrderBy(role => role.Name)
                                .ToListAsync();
            ViewBag.Roles = allRoles;
            return View(user);
        }
        

        public async Task<IActionResult> Details(int id)
        {
            if(id <= 0)
               return NotFound();
            
            var user = await _applicationDbContext.Users
                            .FirstOrDefaultAsync(user => user.Id == id);
            if(user == null)
                return NotFound();

            return View(user);
        }

        public async Task<IActionResult> Edit(int id)
        {
            if(id <= 0)
                return NotFound();

            var user = await _applicationDbContext.Users
                            .Include(user => user.UserRoles)
                            .FirstOrDefaultAsync(user => user.Id == id);

            if(user == null)
                return NotFound();

            var roles = await _applicationDbContext.Roles
                                .OrderBy(role => role.Name)
                                .ToListAsync();
            ViewBag.Roles = roles;
            
            return View(user);
        }
        
        [HttpPost]
        public async Task<IActionResult> Edit(int id, User user, int RoleId)
        {
            if(id != user.Id)
                return NotFound();
            
            if(ModelState.IsValid)
            {
                Console.WriteLine($"Received RoleId: {RoleId} for User ID: {id}");

                var existingUser = await _applicationDbContext.Users
                                                .FirstOrDefaultAsync(user => user.Id == id);
                if(existingUser == null)
                    return NotFound();

                existingUser.Username = user.Username;
                existingUser.Email = user.Email;

                _applicationDbContext.Users.Update(existingUser);

                if(RoleId != existingUser?.UserRoles?.FirstOrDefault()?.RoleId)
                {
                    var existingUserRole = await _applicationDbContext.UserRoles
                                                    .FirstOrDefaultAsync(ur => ur.UserId == id);

                    if(existingUserRole != null)
                    {
                        _applicationDbContext.UserRoles.Remove(existingUserRole);
                        await _applicationDbContext.SaveChangesAsync();
                    }
                    
                    var newUserRole = new  UserRole()
                    {
                        UserId = id,
                        RoleId = RoleId
                    };
                    _applicationDbContext.UserRoles.Add(newUserRole);
                } 
            }
            await _applicationDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            if(id <= 0)
                return NotFound();
            
            var user = await _applicationDbContext.Users
                            .FirstOrDefaultAsync(user => user.Id == id);
            if(user == null)
                return NotFound();

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if(id <= 0)
                return NotFound();

            var user = await _applicationDbContext.Users
                            .FirstOrDefaultAsync(user => user.Id == id);

            if(user == null)
                return NotFound();

            _applicationDbContext.Users.Remove(user);
            await _applicationDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
