using Microsoft.AspNetCore.Mvc;
using School_Management_System.Data;

namespace School_Management_System.Controllers
{
    public class TeacherController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public TeacherController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
    }
}
