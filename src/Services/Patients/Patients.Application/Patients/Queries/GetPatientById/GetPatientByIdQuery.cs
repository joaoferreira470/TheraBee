namespace Patients.Application.Patients.Queries.GetPatientById;

public record GetPatientByIdQuery(PatientId Id) 
    : IQuery<GetPatientByIdResult>;

public record GetPatientByIdResult(PatientDto Patient);