namespace Patients.Application.Dtos;

public record AddSessionGoalsDto(
    IEnumerable<Guid> GoalIds);
