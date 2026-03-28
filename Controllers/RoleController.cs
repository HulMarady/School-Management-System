using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School_Management_System.Data;
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

    }
}
