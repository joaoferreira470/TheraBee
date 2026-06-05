namespace Patients.Application.Dtos;

public record CreateSessionDto(
    DateTime StartDateTime,
    DateTime EndDateTime,
    SessionType Type,
    string Location,
    IEnumerable<Guid>? GoalIds = null);
