namespace School_Management_System.Core.Services;
using School_Management_System.Core.DTOs.Students;
using School_Management_System.Core.Models;

public interface IStudentService
{
    public Task<List<Student>> CreateStudentAsync(CreateStudentDto createStudentDto);
}
