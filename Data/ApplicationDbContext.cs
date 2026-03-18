using Microsoft.EntityFrameworkCore;
using School_Management_System.Models;                                                                                                                          

namespace School_Management_System.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }
    public DbSet<University> Universities { get; set; }
    public DbSet<Department> Departments  { get; set; }
    public DbSet<User> Users { get; set; }
    
}
