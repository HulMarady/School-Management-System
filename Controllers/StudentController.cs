using API.PagedList;
using Microsoft.AspNetCore.Mvc;
using School_Management_System.Data;
using X.PagedList.Extensions;

namespace School_Management_System.Controllers
{
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public StudentController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public ActionResult Index(string? search, int page = 1, int pageSize = 10)
        {
            var query = _applicationDbContext.Students.AsQueryable();

            if(!string.IsNullOrEmpty(search))
            {
                query = query.Where(student => 
                    student.User.FirstName.Contains(search) ||
                    student.User.LastName.Contains(search) ||
                    student.User.Email.Contains(search) ||
                    student.User.Username.Contains(search) ||
                    student.StudentId.Contains(search)
                );
            }

            var students = query.OrderByDescending(student => student.CreatedAt)
                                .ToPagedList(page, pageSize);

            return View(students);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Departments = _applicationDbContext.Departments.ToList();
            return View();
        }
    }
}
