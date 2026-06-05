namespace Patients.Application.Dtos;

public record CompleteSessionDto(
    string ClinicalSummary,
    string ObjectivesWorked,
    string ProgressRating,
    string Activities,
    string PatientResponse,
    string Difficulties,
    string Recommendations,
    string NextSteps);
