using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School_Management_System.Authorization;
using School_Management_System.Data;
using School_Management_System.Models;
using X.PagedList.Extensions;

namespace School_Management_System.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public RoleController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        // [HasPermission("role.view")]
        public ActionResult Index(string? search, int page = 1, int pageSize = 10)
        {
            var query = _applicationDbContext.Roles.AsQueryable();

            if(!string.IsNullOrEmpty(search))
            {
                query = query.Where(r => r.Name.Contains(search));
            }

            var roles = query
                            .OrderBy(role => role.CreatedAt)
                            .ToPagedList(page, pageSize);

            return View(roles);
        }

        // [HasPermission("role.create")]
        public async Task<IActionResult> Create()
        {
            var permissions = await _applicationDbContext.Permissions
                                        .OrderBy(p => p.Name)
                                        .ToListAsync();

            ViewBag.Permissions = permissions;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // [HasPermission("role.create")]
        public async Task<IActionResult> Create(Role role, int[] PermissionIds)
        {
           try
            {
                if(!ModelState.IsValid)
                {
                    return View(role);
                }

                var normalizedRoleName = role.Name.Trim().ToLower();

                bool isExistingRole = await _applicationDbContext.Roles
                                                 .AnyAsync(r => r.Name.ToLower() == normalizedRoleName);

                if(isExistingRole)
                {
                    ModelState.AddModelError(nameof(role.Name), "A role with this name already exists.");
                    return View(role);
                }

                using var transaction = await _applicationDbContext.Database.BeginTransactionAsync();

                role.Name = role.Name.Trim();

                _applicationDbContext.Roles.Add(role);
                await _applicationDbContext.SaveChangesAsync();

                if(PermissionIds != null && PermissionIds.Any())
                {
                    var rolePermissions = PermissionIds.Select(permissionId => new RolePermission
                    {
                        RoleId = role.Id,
                        PermissionId = permissionId
                    }).ToList();

                    _applicationDbContext.RolePermissions.AddRange(rolePermissions);
                    await _applicationDbContext.SaveChangesAsync();
                }

                await transaction.CommitAsync();

                return RedirectToAction(nameof(Index));
            } 
            catch(Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An error occurred while creating the role: {ex.Message}");
                return View(role);
            }
        }

        [HasPermission("role.view")]
        public async Task<IActionResult> Details(int id)
        {
            if(id < 0)
                return NotFound(); 

            var role = await _applicationDbContext.Roles.FirstOrDefaultAsync(role => role.Id == id);

            if(role is null)
                return NotFound();

            return View(role);
        }
        // [HasPermission("role.edit")]
        public async Task<IActionResult> Edit(int id)
        {
            if(id < 0)
                return NotFound();

            var role = await _applicationDbContext.Roles
                                .Include(r => r.RolesPermissions)
                                .FirstOrDefaultAsync(r => r.Id == id);
            
            if(role is null)
                return NotFound();

            ViewBag.Permissions = await _applicationDbContext.Permissions
                                        .OrderBy(p => p.Name)
                                        .ToListAsync();
            return View(role); 
        }

        [HttpPost]
        // [HasPermission("role.edit")]
        public async Task<IActionResult> Edit(Role role, int id, int[] PermissionIds)
        {
            if(!ModelState.IsValid)
            {
                return View(role);
            }

            if(id != role.Id)
                return NotFound();
            
            if(await _applicationDbContext.Roles.AnyAsync(r => r.Name == role.Name && r.Id != id))
            {
                ModelState.AddModelError(nameof(role.Name), "A role with this name already exists.");
                return View(role);
            }

            using var transaction = await _applicationDbContext.Database.BeginTransactionAsync();

            var existingRole = await _applicationDbContext.Roles
                                        .Include(rp => rp.RolesPermissions)
                                        .FirstOrDefaultAsync(r => r.Id == id);

            if(existingRole is null)
                return NotFound();

            existingRole.Name = role.Name.Trim();
            _applicationDbContext.Roles.Update(existingRole);
            await _applicationDbContext.SaveChangesAsync();

            if(existingRole.RolesPermissions != null && existingRole.RolesPermissions.Any())
            {
                _applicationDbContext.RolePermissions.RemoveRange(existingRole.RolesPermissions);
                await _applicationDbContext.SaveChangesAsync();
            }

            if(PermissionIds != null && PermissionIds.Any())
            {
                var rolePermissions = PermissionIds.Select(permissionId => new RolePermission
                {
                    RoleId = existingRole.Id,
                    PermissionId = permissionId
                }).ToList();

                _applicationDbContext.RolePermissions.AddRange(rolePermissions);
                await _applicationDbContext.SaveChangesAsync();

                await transaction.CommitAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        [HasPermission("role.delete")]
        public async Task<IActionResult> Delete(int id)
        {
            if(id < 0)
                return NotFound();

            var role = await _applicationDbContext.Roles.FirstOrDefaultAsync(role => role.Id == id);

            if(role is null)
                return NotFound();

            return View(role);
       }

       [HttpPost, ActionName("Delete")]
    //    [HasPermission("role.delete")]
       public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var role = await _applicationDbContext.Roles.FirstOrDefaultAsync(role => role.Id == id);

            if(role is null)
                return NotFound();

            _applicationDbContext.Roles.Remove(role);
            await _applicationDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
