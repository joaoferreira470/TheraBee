namespace Patients.Application.Extensions;

public static class TherapistProfileExtensions
{
    public static TherapistProfileDto ToDto(this TherapistProfile profile)
    {
        return new TherapistProfileDto(
            profile.Id,
            profile.UserId,
            profile.ProfessionalName,
            profile.Profession,
            profile.Specialties,
            profile.ProfessionalNumber,
            profile.PhoneNumber,
            profile.Workplace,
            profile.ReportSignature,
            profile.PortraitStorageKey is not null,
            profile.PortraitUpdatedAt);
    }
}

