namespace Patients.Application.Therapists.Queries.GetCurrentTherapist;

public record GetCurrentTherapistQuery()
    : IQuery<GetCurrentTherapistResult>;

public record GetCurrentTherapistResult(TherapistProfileDto Therapist);

