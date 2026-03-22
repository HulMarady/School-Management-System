using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace School_Management_System.Controllers
{
    [Authorize] 
    public class DashboardController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
