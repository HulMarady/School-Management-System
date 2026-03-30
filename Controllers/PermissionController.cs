using Microsoft.AspNetCore.Mvc;
using School_Management_System.Data;
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
    }
}
