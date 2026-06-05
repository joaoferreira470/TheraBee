namespace Patients.Application.Dtos;

public record PatientProgressDashboardDto(
    PatientDto Patient,
    AttendanceMetricsDto Attendance,
    GoalProgressMetricsDto Goals,
    SessionDto? LastSession,
    SessionDto? NextSession,
    DateTime GeneratedAt);
