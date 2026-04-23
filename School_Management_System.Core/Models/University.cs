using System.ComponentModel.DataAnnotations;

namespace School_Management_System.Core.Models;

public class University
{
    public int Id { get; set; }

    [Required(ErrorMessage = "{0} is required")]
    [Display(Name = "University Name")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "{0} must be between {2} and {1} characters")]
    public string Name { get; set; }

    [Required(ErrorMessage = "{0} is required")]
    [EmailAddress]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "{0} must be between {2} and {1} characters")]
    public string Email { get; set; }
    public string? Location { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}
