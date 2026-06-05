namespace Patients.Domain.Models;

public class User : Entity<Guid>
{
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;
    public string Role { get; set; } = "Therapist";
}

