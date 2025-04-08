namespace Patients.Application.Patients.Queries.GetPatientsByTherapist;

public record GetPatientsByTherapistQuery(Guid TherapistId)
    : IQuery<GetPatientsByTherapistResult>;


public record GetPatientsByTherapistResult(IEnumerable<PatientDto> Patients);