using Microsoft.AspNetCore.Mvc;
using School_Management_System.Data;
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
            // var query = _applicationDbContext.Courses.AsQueryable(); 
            return View();
        }

    }
}
