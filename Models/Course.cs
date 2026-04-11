using System.ComponentModel.DataAnnotations;

namespace School_Management_System.Models;
public class Course
{
    public int Id { get; set; } 

    public string Name { get; set; } 
    
    public string Credits { get; set; }

    public int DepartmentId { get; set; }
    public Department Department { get; set; }
} 
