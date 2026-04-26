using Microsoft.EntityFrameworkCore;
using School_Management_System.Core.Models;                                                                                                                          

namespace School_Management_System.Infrastructure.Data;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }
    public DbSet<University> Universities { get; set; }
    public DbSet<Department> Departments  { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Teacher> Teachers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Custome Table Names
        modelBuilder.Entity<RolePermission>().ToTable("role_permissions");
        modelBuilder.Entity<UserRole>().ToTable("user_roles");

        // Configure UserRole composite key
        modelBuilder.Entity<UserRole>()
            .HasKey(ur => new { ur.UserId, ur.RoleId });

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId);

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId);

        modelBuilder.Entity<RolePermission>()
            .HasKey(rp => new { rp.RoleId, rp.PermissionId });

        modelBuilder.Entity<RolePermission>()
            .HasOne(rp => rp.Role)
            .WithMany(r => r.RolesPermissions)
            .HasForeignKey(rp => rp.RoleId);

        modelBuilder.Entity<RolePermission>()
            .HasOne(rp => rp.Permission)
            .WithMany(p => p.RolesPermissions)
            .HasForeignKey(rp => rp.PermissionId);
    }

    public override int SaveChanges()
    {
        GenerateStudentId();
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        GenerateStudentId();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void GenerateStudentId()
    {
        var newStudents = ChangeTracker.Entries<Student>()
            .Where(entry => entry.State == EntityState.Added)
            .Select(entry => entry.Entity);
        
        foreach(var student in newStudents)
        {
            if(string.IsNullOrEmpty(student.StudentId))
            {
                var year = DateTime.Now.Year;

                var lastStudent = Students
                    .Where(student => student.StudentId.StartsWith(year.ToString()))
                    .OrderByDescending(student => student.Id)
                    .FirstOrDefault();

                int nextNumber = 1;

                if(lastStudent is not null)
                {
                    var lastNumber = int.Parse(lastStudent.StudentId.Substring(7));
                    nextNumber = lastNumber + 1;
                }

                student.StudentId = $"IDT{year}-{nextNumber.ToString("D5")}";
            }
        }

    }
}
