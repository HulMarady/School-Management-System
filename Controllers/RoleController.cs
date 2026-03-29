using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School_Management_System.Data;
using School_Management_System.Models;
using X.PagedList.Extensions;

namespace School_Management_System.Controllers
{
    [Authorize]
    public class RoleController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public RoleController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public ActionResult Index(string? search, int page = 1, int pageSize = 10)
        {
            var query = _applicationDbContext.Roles.AsQueryable();

            if(!string.IsNullOrEmpty(search))
            {
                query = query.Where(r => r.Name.Contains(search));
            }

            var roles = query
                            .OrderBy(role => role.CreatedAt)
                            .ToPagedList(page, pageSize);

            return View(roles);
       }
       public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Role role)
        {
            if(ModelState.IsValid)
            {
                if(_applicationDbContext.Roles.Any(r => r.Name == role.Name))
                {
                    ModelState.AddModelError(nameof(role.Name), "A role with this name already exists.");
                    return View(role);
                }
                _applicationDbContext.Roles.Add(role);
                await _applicationDbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(role);
        }

        public async Task<IActionResult> Detaisl(int id)
        {
            if(id < 0)
                return NotFound(); 

            var role = _applicationDbContext.Roles.FirstOrDefault(role => role.Id == id);

            if(role is null)
                return NotFound();

            return View(role);
        }
    }
}
