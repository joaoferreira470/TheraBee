namespace Patients.Application.Sessions.Queries.GetSessionsByPatient;

public record GetSessionsByPatientQuery(Guid PatientId)
    : IQuery<GetSessionsByPatientResult>;

public record GetSessionsByPatientResult(IEnumerable<SessionDto> Sessions);
