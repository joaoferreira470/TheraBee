namespace Patients.Application.Therapists.Queries.GetCurrentTherapistPortrait;

public record GetCurrentTherapistPortraitQuery() : IQuery<GetCurrentTherapistPortraitResult>;

public record GetCurrentTherapistPortraitResult(PortraitContentDto Portrait);
