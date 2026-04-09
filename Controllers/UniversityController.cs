using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School_Management_System.Data;
using School_Management_System.Models;
using X.PagedList.Extensions;

namespace School_Management_System.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UniversityController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public UniversityController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<IActionResult> Index(string? search, int page = 1, int pageSize = 10)
        {
            var query = _applicationDbContext.Universities.AsQueryable();

            if(!string.IsNullOrEmpty(search))
            {
                query = query.Where(university =>
                    university.Name.Contains(search) ||
                    university.Email.Contains(search) ||
                    university.Location.Contains(search)
                );
            }

            var universities = query.OrderByDescending(university => university.CreatedAt)
                                        .ToPagedList(page, pageSize);

            return View(universities);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(University university)
        {
            if(!ModelState.IsValid)
            {
                foreach(var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                return View(university);
            }
            _applicationDbContext.Universities.Add(university);
            await _applicationDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
        public async Task<IActionResult> Edit(int id)
        {
            if(id <= 0)
            return NotFound();
        
            var university = await _applicationDbContext.Universities
                                        .FindAsync(id);

            if(university is null)
                return NotFound();

            return View(university);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, University university)
        {
            if(id != university.Id)
                return NotFound();

            var existingUniversity = await _applicationDbContext.Universities
                                        .FindAsync(id);
            if(existingUniversity is null)
                return NotFound();

            _applicationDbContext.Entry(existingUniversity).CurrentValues.SetValues(university);
            await _applicationDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            if(id <=0)
                return NotFound();

            var university = await _applicationDbContext.Universities
                                        .FindAsync(id);

            if(university is null)
                return NotFound();

            return View(university);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if(id <= 0)
                return NotFound();
            
            var university = _applicationDbContext.Universities
                                .FirstOrDefault(university => university.Id == id);

            if(university is null)
                return NotFound();

            _applicationDbContext.Universities.Remove(university);
            await _applicationDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
