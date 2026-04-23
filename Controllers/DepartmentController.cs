using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School_Management_System.Infrastructure.Data;
using School_Management_System.Core.Models;
using X.PagedList.Extensions;

namespace School_Management_System.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public DepartmentController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public ActionResult Index(string? search, int page = 1, int pageSize = 10)
        {
            var query = _applicationDbContext.Departments.AsQueryable();  

            if(!string.IsNullOrEmpty(search))
            {
                query = query.Where(department => department.Name.Contains(search));
            }

            var departments = query.OrderByDescending(department => department.CreatedAt)
                                        .Include(department => department.University)
                                        .ToPagedList(page, pageSize);
            
            return View(departments);
        }

        public async Task<IActionResult> Create()
        {
            var universities = await _applicationDbContext.Universities.ToListAsync();
            ViewBag.Universities = universities;
            return View();
        }

        public async Task<IActionResult> Edit(int id)
        {
            if(id <= 0)
                return NotFound();

            var department = await _applicationDbContext.Departments
                                        .Include(department => department.University)
                                        .FirstOrDefaultAsync(department => department.Id == id);
            
            if(department == null)
                return NotFound();

            var universities = await _applicationDbContext.Universities.ToListAsync();
            ViewBag.Universities = universities;

            return View(department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Department department)
        {
            if(id != department.Id)
                return NotFound();

            if(!ModelState.IsValid)
            {
                var universities = await _applicationDbContext.Universities.ToListAsync();
                ViewBag.Universities = universities;
                return View(department);
            }

            _applicationDbContext.Departments.Update(department);
            await _applicationDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            if(id <= 0)
                return NotFound();

            var department = _applicationDbContext.Departments
                                .Include(department => department.University)
                                .FirstOrDefault(department => department.Id == id);

            if(department == null)
                return NotFound();

            return View(department);
        }

        public async Task<IActionResult> Delete(int id)
        {
            if(id <= 0)
                return NotFound();

            var department = _applicationDbContext.Departments
                                .Include(department => department.University)
                                .FirstOrDefault(department => department.Id == id);
            
            if(department is null)
                return NotFound();

            return View(department);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if(id <= 0)
                return NotFound();

            var department = await _applicationDbContext.Departments
                                .FirstOrDefaultAsync(department => department.Id == id);

            if(department is null)
                return NotFound();

            _applicationDbContext.Departments.Remove(department);
            await _applicationDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
