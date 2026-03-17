using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace School_Management_System.Models;

public class Department
{
    public int Id { get; set; }

    [Required(ErrorMessage = "{0} is required")]
    [StringLength(120, MinimumLength = 6, ErrorMessage = "{0} must be between {2} and {1} characters")]
    public string Name { get; set; }
    public int UniversityId { get; set; }
    public University University { get; set; }
}
