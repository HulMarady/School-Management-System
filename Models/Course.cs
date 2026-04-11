using System.ComponentModel.DataAnnotations;

namespace School_Management_System.Models;
public class Course
{
    public int Id { get; set; } 

    [Required]
    [Display(Name = "Course Name")]
    public string Name { get; set; } 
    
    [Required]
    [Display(Name = "Credits")]
    public string Credits { get; set; }

    public int DepartmentId { get; set; }
    public Department Department { get; set; }
} 
