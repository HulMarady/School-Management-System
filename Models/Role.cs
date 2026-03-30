using System.ComponentModel.DataAnnotations;

namespace School_Management_System.Models;

public class Role
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Role Name")]
    [StringLength(50, MinimumLength = 3)]
    [RegularExpression(@"^[a-zA-Z0-9_ ]+$", ErrorMessage = "Role name can only contain letters, numbers, spaces, and underscores.")]
    public string Name { get; set; } 
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>(); 
    public ICollection<RolePermission> RolesPermissions { get; set; } = new List<RolePermission>();
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

}
