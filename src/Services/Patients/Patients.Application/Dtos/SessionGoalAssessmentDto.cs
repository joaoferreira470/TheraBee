namespace Patients.Application.Dtos;

public record SessionGoalAssessmentDto(
    Guid Id,
    Guid SessionId,
    Guid TherapeuticGoalId,
    int Score,
    string? ClinicalNotes,
    DateTime? SessionStartDateTime,
    DateTime? CreatedAt);
