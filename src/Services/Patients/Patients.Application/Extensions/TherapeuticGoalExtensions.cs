namespace Patients.Application.Extensions;

public static class TherapeuticGoalExtensions
{
    public static TherapeuticGoalDto ToTherapeuticGoalDto(this TherapeuticGoal goal)
    {
        return new TherapeuticGoalDto(
            Id: goal.Id,
            PatientId: goal.PatientId,
            TherapistId: goal.TherapistId,
            Type: goal.Type,
            Description: goal.Description,
            Priority: goal.Priority,
            Status: goal.Status,
            CreatedAt: goal.CreatedAt);
    }

    public static IEnumerable<TherapeuticGoalDto> ToTherapeuticGoalDtoList(this IEnumerable<TherapeuticGoal> goals)
    {
        return goals.Select(ToTherapeuticGoalDto);
    }
}
