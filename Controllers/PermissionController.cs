using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School_Management_System.Infrastructure.Data;
using School_Management_System.Models;
using X.PagedList.Extensions;

namespace School_Management_System.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PermissionController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public PermissionController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<IActionResult> Index(string? search, int page = 1, int pageSize = 10)
        {
            var query = _applicationDbContext.Permissions.AsQueryable();

            if(!string.IsNullOrEmpty(search))
            {
                query = query.Where(permission => permission.Name.Contains(search));
            }

            var permissions = query.OrderByDescending(permission => permission.CreatedAt)
                                   .ToPagedList(page, pageSize);

            return View(permissions);
        }
        
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Permission permission)
        {
            if(!ModelState.IsValid)
            {
                foreach(var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                return View(permission);
            }

            bool isExistinngPermission = await _applicationDbContext.Permissions
                                                    .AnyAsync(p => p.Name == permission.Name);
            
            if(isExistinngPermission)
            {
                ModelState.AddModelError(nameof(permission.Name), "A permission with this name already exists.");
                return View(permission);
            }

            if(ModelState.IsValid)
            {
                _applicationDbContext.Permissions.Add(permission);
                await _applicationDbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(permission);
        }

        public async Task<IActionResult> Details(int id)
        {
            if(id < 0)
                return NotFound();
            
            var permission = await _applicationDbContext.Permissions
                                        .Include(p => p.RolesPermissions)
                                            .ThenInclude(rp => rp.Role)
                                        .FirstOrDefaultAsync(p => p.Id == id);

            if(permission is null)
                return NotFound();

            return View(permission);
        }

        public async Task<IActionResult> Edit(int id)
        {
            if(id < 0)
                return NotFound();

            var permission = await _applicationDbContext.Permissions
                                        .FirstOrDefaultAsync(p => p.Id == id);
            if(permission is null)
                return NotFound();

            return View(permission);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Permission permission, int id)
        {
            if(id < 0)
                return NotFound();

            if(id != permission.Id)
                return NotFound();

            if(!ModelState.IsValid)
            {
                return View(permission);
            }
            bool isExistingPermission = await _applicationDbContext.Permissions
                                                    .AnyAsync(p => p.Name == permission.Name);

            if(isExistingPermission)
            {
                ModelState.AddModelError(nameof(permission.Name), "A permission with this name already exists.");
                return View(permission);
            }

            _applicationDbContext.Permissions.Update(permission);
            await _applicationDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            if(id < 0)
                return NotFound();

            var permission = await _applicationDbContext.Permissions
                                        .FirstOrDefaultAsync(p => p.Id == id);
            if(permission is null)
                return NotFound();

            return View(permission);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Permission permission, int id)
        {
            if(id < 0)
                return NotFound();

            if(id != permission.Id)
                return NotFound();

            _applicationDbContext.Permissions.Remove(permission);
            await _applicationDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}