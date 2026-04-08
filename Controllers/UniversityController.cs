using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

    }
}
