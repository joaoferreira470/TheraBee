namespace Patients.Application.Dtos;

public record SetSessionGoalsDto(
    IEnumerable<Guid> GoalIds);
