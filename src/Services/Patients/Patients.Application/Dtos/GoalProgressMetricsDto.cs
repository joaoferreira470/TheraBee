namespace Patients.Application.Dtos;

public record GoalProgressMetricsDto(
    int TotalGoals,
    int NotStartedGoals,
    int InProgressGoals,
    int AchievedGoals,
    int SuspendedGoals,
    decimal CompletionRate);
