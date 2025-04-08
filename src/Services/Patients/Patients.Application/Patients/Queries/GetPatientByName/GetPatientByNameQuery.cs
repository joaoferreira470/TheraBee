namespace Patients.Application.Patients.Queries.GetPatientByName;

public record GetPatientByNameQuery(string Name)
    : IQuery<GetPatientByNameResult>;

public record GetPatientByNameResult(PatientDto Patient);