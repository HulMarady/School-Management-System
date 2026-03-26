namespace School_Management_System.Models;

public class Role
{
    public int Id { get; set; }
    public string Name { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

}
