using API.PagedList;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School_Management_System.Data;
using X.PagedList.Extensions;

namespace School_Management_System.Controllers
{
    public class TeacherController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public TeacherController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<IActionResult> Index(string? search, int page = 1, int pageSize = 10)
        {
            var query = _applicationDbContext.Teachers.AsQueryable();

            if(!string.IsNullOrEmpty(search))
            {
                query = query.Where(teacher => 
                    teacher.User.FirstName.Contains(search) || 
                    teacher.User.LastName.Contains(search) || 
                    teacher.User.Email.Contains(search) ||
                    teacher.TeacherId.Contains(search)
                );
            }

            var teachers = await query.OrderByDescending(teacher => teacher.CreatedAt)
                                      .Include(teacher => teacher.User)
                                      .Include(teacher => teacher.Department)
                                      .ToPagedListAsync(page, pageSize);

            return View();
        }
        
        public async Task<IActionResult> Create()
        {
            ViewBag.Departments = await _applicationDbContext.Departments.ToListAsync();
            return View();
        }
    }
}
