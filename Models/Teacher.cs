namespace School_Management_System.Models;

public class Teacher
{
    public int Id { get; set; }
    public string TeacherId { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public string? Qualification { get; set; }   
    public string? Specialization { get; set; }
    public DateTime HireDate { get; set; }  
    public int? UserId { get; set; }
    public User User { get; set; }
    public int? DepartmentId { get; set;  }
    public Department Department { get; set;}
}
