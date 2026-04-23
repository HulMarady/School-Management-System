using System.ComponentModel.DataAnnotations;

namespace School_Management_System.Core.Models;

public class Student
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    [RegularExpression(@"^[a-zA-Z]+$")]
    public string Gender { get; set; }  

    [Display(Name = "Date of Birth")]
    public DateTime DateOfBirth { get; set; }
    
    [Display(Name = "Student ID")]
    public string? StudentId { get; set; }

    [Display(Name = "Phone Number")]
    public string? PhoneNumber { get; set; } 
    public string? Address { get; set; }


    public int UserId { get; set; }
    public User User { get; set; }
    public int DepartmentId { get; set; }
    public Department Department { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

}
