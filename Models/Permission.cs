namespace School_Management_System.Models;

public class Permission
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<RolePermission> RolesPermissions { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}
