using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School_Management_System.Data;
using School_Management_System.Models;
using X.PagedList.Extensions;

namespace School_Management_System.Controllers
{
    public class CourseController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public CourseController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public ActionResult Index(string? search, int page =1, int pageSize = 10)
        {
            var query = _applicationDbContext.Courses.AsQueryable(); 

            if(!string.IsNullOrEmpty(search))
            {
                query = query.Where(course => course.Name.Contains(search));
            }

            var course = query.OrderByDescending(course => course.Name)
                              .ToPagedList(page, pageSize);

            return View(course);
        }

        public async Task<IActionResult> Create()
        {
            var departments = await _applicationDbContext.Departments.ToListAsync();
            ViewBag.Departments = departments;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Course course, int DepartmentId)
        {
            if(!ModelState.IsValid)
            {
                var departments = await _applicationDbContext.Departments.ToListAsync();
                ViewBag.Departments = departments;

                return View(course);
            }

            _applicationDbContext.Courses.Add(course);
            await _applicationDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
