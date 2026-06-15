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
    public string? PortraitStorageKey { get; set; }
    public string? PortraitContentType { get; set; }
    public DateTime? PortraitUpdatedAt { get; set; }

    public void SetPortrait(string storageKey, string contentType, DateTime updatedAt)
    {
        PortraitStorageKey = storageKey;
        PortraitContentType = contentType;
        PortraitUpdatedAt = updatedAt;
    }

    public void RemovePortrait()
    {
        PortraitStorageKey = null;
        PortraitContentType = null;
        PortraitUpdatedAt = null;
    }
}

