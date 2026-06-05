namespace Patients.Application.Patients.Queries.GetPatientsByTherapist;

public record GetPatientsByTherapistQuery()
    : IQuery<GetPatientsByTherapistResult>;

public record GetPatientsByTherapistResult(IEnumerable<PatientDto> Patients);
