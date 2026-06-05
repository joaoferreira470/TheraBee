namespace Patients.Application.Dtos;

public record AttendanceMetricsDto(
    int TotalSessions,
    int CompletedSessions,
    int CancelledSessions,
    int NoShowSessions,
    int UpcomingSessions,
    int PendingRegistrationSessions,
    decimal AttendanceRate);
