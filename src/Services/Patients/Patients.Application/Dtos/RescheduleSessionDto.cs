namespace Patients.Application.Dtos;

public record RescheduleSessionDto(
    DateTime StartDateTime,
    DateTime EndDateTime,
    SessionType Type,
    string Location);
