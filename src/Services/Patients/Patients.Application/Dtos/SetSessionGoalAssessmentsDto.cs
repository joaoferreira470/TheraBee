namespace Patients.Application.Dtos;

public record SessionGoalAssessmentInputDto(
    Guid TherapeuticGoalId,
    int Score,
    string? ClinicalNotes);

public record SetSessionGoalAssessmentsDto(
    IEnumerable<SessionGoalAssessmentInputDto> Assessments);
