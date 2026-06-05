namespace Patients.Application.Extensions;

public static class TherapeuticGoalExtensions
{
    public static TherapeuticGoalDto ToTherapeuticGoalDto(this TherapeuticGoal goal)
    {
        return new TherapeuticGoalDto(
            Id: goal.Id,
            PatientId: goal.PatientId,
            TherapistId: goal.TherapistId,
            Description: goal.Description,
            Area: goal.Area,
            Priority: goal.Priority,
            Status: goal.Status,
            ReviewDate: goal.ReviewDate,
            CreatedAt: goal.CreatedAt);
    }

    public static IEnumerable<TherapeuticGoalDto> ToTherapeuticGoalDtoList(this IEnumerable<TherapeuticGoal> goals)
    {
        return goals.Select(ToTherapeuticGoalDto);
    }
}
