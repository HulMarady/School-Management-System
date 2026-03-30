using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School_Management_System.Data;
using School_Management_System.Models;
using X.PagedList.Extensions;

namespace School_Management_System.Controllers
{
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
                query.Where(permission => permission.Name.Contains(search));
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

    }
}
