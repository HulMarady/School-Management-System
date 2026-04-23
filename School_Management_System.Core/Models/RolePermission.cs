using School_Management_System.Models;

namespace School_Management_System.Core.Models;

public class RolePermission
{
    public int RoleId { get; set; }
    public Role Role { get; set; }
    public int PermissionId { get; set; }
    public Permission Permission { get; set; }
}
