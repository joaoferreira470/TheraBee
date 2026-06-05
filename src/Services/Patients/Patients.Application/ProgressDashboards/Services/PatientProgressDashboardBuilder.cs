namespace Patients.Application.ProgressDashboards.Services;

public class PatientProgressDashboardBuilder(IApplicationDbContext dbContext) : IPatientProgressDashboardBuilder
{
    public async Task<PatientProgressDashboardDto> BuildAsync(Guid patientId, Guid therapistId, CancellationToken cancellationToken)
    {
        var patient = await dbContext.Patients
            .AsNoTracking()
            .FirstOrDefaultAsync(patient => patient.Id == PatientId.Of(patientId) && patient.TherapistId == therapistId, cancellationToken);

        if (patient == null)
        {
            throw new PatientNotFoundException(patientId);
        }

        var sessions = await dbContext.Sessions
            .AsNoTracking()
            .Include(session => session.SessionGoals)
            .Where(session => session.PatientId == patientId && session.TherapistId == therapistId)
            .ToListAsync(cancellationToken);

        var goals = await dbContext.TherapeuticGoals
            .AsNoTracking()
            .Where(goal => goal.PatientId == patientId && goal.TherapistId == therapistId)
            .ToListAsync(cancellationToken);

        var now = DateTime.UtcNow;

        var completedSessions = sessions.Count(session => session.Status == SessionStatus.Completed);
        var cancelledSessions = sessions.Count(session => session.Status == SessionStatus.Cancelled);
        var noShowSessions = sessions.Count(session => session.Status is SessionStatus.PatientNoShow or SessionStatus.TherapistNoShow);
        var upcomingSessions = sessions.Count(session =>
            (session.Status is SessionStatus.Scheduled or SessionStatus.Rescheduled)
            && session.StartDateTime > now);
        var pendingRegistrationSessions = sessions.Count(session =>
            (session.Status is SessionStatus.Scheduled or SessionStatus.Rescheduled)
            && session.EndDateTime <= now);

        var attendanceBase = completedSessions + noShowSessions;
        var attendanceRate = attendanceBase == 0
            ? 0m
            : Math.Round((decimal)completedSessions * 100m / attendanceBase, 1);

        var totalGoals = goals.Count;
        var notStartedGoals = goals.Count(goal => goal.Status == TherapeuticGoalStatus.NotStarted);
        var inProgressGoals = goals.Count(goal => goal.Status == TherapeuticGoalStatus.InProgress);
        var achievedGoals = goals.Count(goal => goal.Status == TherapeuticGoalStatus.Achieved);
        var suspendedGoals = goals.Count(goal => goal.Status == TherapeuticGoalStatus.Suspended);
        var goalCompletionRate = totalGoals == 0
            ? 0m
            : Math.Round((decimal)achievedGoals * 100m / totalGoals, 1);

        var lastSession = sessions
            .Where(session => session.StartDateTime <= now)
            .OrderByDescending(session => session.StartDateTime)
            .FirstOrDefault();

        var nextSession = sessions
            .Where(session => session.StartDateTime > now
                && session.Status is SessionStatus.Scheduled or SessionStatus.Rescheduled)
            .OrderBy(session => session.StartDateTime)
            .FirstOrDefault();

        return new PatientProgressDashboardDto(
            Patient: patient.ToPatientDto(),
            Attendance: new AttendanceMetricsDto(
                TotalSessions: sessions.Count,
                CompletedSessions: completedSessions,
                CancelledSessions: cancelledSessions,
                NoShowSessions: noShowSessions,
                UpcomingSessions: upcomingSessions,
                PendingRegistrationSessions: pendingRegistrationSessions,
                AttendanceRate: attendanceRate),
            Goals: new GoalProgressMetricsDto(
                TotalGoals: totalGoals,
                NotStartedGoals: notStartedGoals,
                InProgressGoals: inProgressGoals,
                AchievedGoals: achievedGoals,
                SuspendedGoals: suspendedGoals,
                CompletionRate: goalCompletionRate),
            LastSession: lastSession?.ToSessionDto(),
            NextSession: nextSession?.ToSessionDto(),
            GeneratedAt: DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc));
    }
}
