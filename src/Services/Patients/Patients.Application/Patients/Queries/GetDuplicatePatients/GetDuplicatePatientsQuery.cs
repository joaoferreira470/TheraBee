namespace Patients.Application.Patients.Queries.GetDuplicatePatients;

public record GetDuplicatePatientsQuery(string Name, DateTime DateOfBirth)
    : IQuery<GetDuplicatePatientsResult>;

public record GetDuplicatePatientsResult(IEnumerable<PatientDto> Patients);
