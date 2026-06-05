namespace Patients.Domain.Models;

public class TherapistProfile : Entity<Guid>
{
    public Guid UserId { get; set; }
    public string ProfessionalName { get; set; } = default!;
    public string Profession { get; set; } = default!;
    public string? Specialties { get; set; }
    public string? ProfessionalNumber { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Workplace { get; set; }
    public string? ReportSignature { get; set; }
}

