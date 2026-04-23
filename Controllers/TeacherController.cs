using API.PagedList;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School_Management_System.Infrastructure.Data;
using School_Management_System.Core.Models;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Teacher teacher)
        {
            if(ModelState.IsValid)
            {
                _applicationDbContext.Teachers.Add(teacher);
                await _applicationDbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Departments = await _applicationDbContext.Departments.ToListAsync();
            return View(teacher);
        }

        public async Task<IActionResult> Details(int id)
        {
            if(id <= 0)
                return NotFound(); 

            var teacher = _applicationDbContext.Teachers   
                .Include(teacher => teacher.User)
                .Include(teacher => teacher.Department)
                .FirstOrDefault(teacher => teacher.Id == id);

            if(teacher is null)
                return NotFound();

            return View(teacher);
        }

        public async Task<IActionResult> Edit(int id)
        {
            if(id <= 0)
                return NotFound();

            var teacher = await _applicationDbContext.Teachers
                .Include(teacher => teacher.User)
                .Include(teacher => teacher.Department)
                .FirstOrDefaultAsync(teacher => teacher.Id == id);

            if(teacher is null)
                return NotFound();

            ViewBag.Departments = await _applicationDbContext.Departments.ToListAsync();
            return View(teacher);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Teacher teacher)
        {
            if(id != teacher.Id)
                return NotFound();

            var existingTeacher = await _applicationDbContext.Teachers
                .Include(teacher => teacher .User)
                .Include(teacher => teacher.Department)
                .FirstOrDefaultAsync(teacher => teacher.Id == id);

            if(existingTeacher is null)
                return NotFound();

            _applicationDbContext.Entry(existingTeacher).CurrentValues.SetValues(teacher);
            await _applicationDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            if(id <= 0)
                return NotFound();

            var teacher = _applicationDbContext.Teachers.Find(id);

            if(teacher is null)
                return NotFound();

            return View(teacher);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if(id <= 0)
                return NotFound();
            
            var teacher = _applicationDbContext.Teachers.Find(id);

            if(teacher is null)
                return NotFound();

            _applicationDbContext.Teachers.Remove(teacher);
            await _applicationDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
