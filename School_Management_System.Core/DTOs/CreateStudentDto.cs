namespace School_Management_System.Core.DTOs.Students;
public class CreateStudentDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public DateTime DateOfBirth { get; set; }
    public int DepartmentId { get; set; }
}
