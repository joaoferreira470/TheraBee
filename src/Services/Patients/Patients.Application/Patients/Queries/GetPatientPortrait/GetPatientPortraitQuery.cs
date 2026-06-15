namespace Patients.Application.Patients.Queries.GetPatientPortrait;

public record GetPatientPortraitQuery(Guid PatientId) : IQuery<GetPatientPortraitResult>;

public record GetPatientPortraitResult(PortraitContentDto Portrait);
