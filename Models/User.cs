using System.ComponentModel.DataAnnotations;

namespace School_Management_System.Models;

public class User
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "First Name")]
    [StringLength(50, MinimumLength = 2)]
    [RegularExpression("^[a-zA-Z ]+$")]
    public string FirstName { get; set; }
    [Required]
    [Display(Name = "Last Name")]
    [StringLength(50, MinimumLength = 2)]
    [RegularExpression("^[a-zA-Z ]+$")]
    public string LastName { get; set; }

    [Required]
    [Display(Name = "Username")]
    [StringLength(100, MinimumLength = 6)]
    [RegularExpression(@"^[a-zA-Z0-9_ ]+$")]
    public string Username { get; set; }

    [Required, EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; }

    [Required]
    [Display(Name = "Password")]
    [StringLength(100, MinimumLength = 6)]
    public string Password { get; set; }
    public ICollection<UserRole> UserRoles { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}
