namespace Patients.Application.Dtos;

public record SessionDto(
    Guid Id,
    Guid PatientId,
    Guid TherapistId,
    DateTime StartDateTime,
    DateTime EndDateTime,
    SessionType Type,
    string Location,
    SessionStatus Status,
    string? CancellationReason,
    string? ClinicalSummary,
    string? ObjectivesWorked,
    string? ProgressRating,
    string? Activities,
    string? PatientResponse,
    string? Difficulties,
    string? Recommendations,
    string? NextSteps,
    IEnumerable<Guid> GoalIds,
    IEnumerable<SessionGoalAssessmentDto> GoalAssessments,
    DateTime? CreatedAt);
