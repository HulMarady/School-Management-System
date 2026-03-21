using System.ComponentModel.DataAnnotations;

namespace School_Management_System.Models;

public class User
{
    public int Id { get; set; }
    [Required]
    public string Username { get; set; }

    [Required, EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
    public string? Role { get; set; } // e.g., "Admin", "Student", "Teacher"
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}
