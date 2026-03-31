using System.ComponentModel.DataAnnotations;

namespace School_Management_System.Models;

public class Permission
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Permission Name")]
    [StringLength(50, MinimumLength = 3)]
    public string Name { get; set; }  
    public ICollection<RolePermission> RolesPermissions { get; set; } = new List<RolePermission>();
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}
