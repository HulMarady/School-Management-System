using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School_Management_System.Data;
using API.PagedList;
using Microsoft.EntityFrameworkCore;

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
    }
}
