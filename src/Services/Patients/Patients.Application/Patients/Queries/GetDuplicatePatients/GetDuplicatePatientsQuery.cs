namespace Patients.Application.Patients.Queries.GetDuplicatePatients;

public record GetDuplicatePatientsQuery(string Name, DateTime DateOfBirth, string? PhoneNumber = null, string? Email = null)
    : IQuery<GetDuplicatePatientsResult>;

public record GetDuplicatePatientsResult(IEnumerable<PatientDto> Patients);
