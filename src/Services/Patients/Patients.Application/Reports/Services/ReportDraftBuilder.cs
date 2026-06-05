using System.Globalization;

namespace Patients.Application.Reports.Services;

public class ReportDraftBuilder(IApplicationDbContext dbContext, IPatientProgressDashboardBuilder dashboardBuilder) : IReportDraftBuilder
{
    public async Task<Report> BuildAsync(Guid patientId, Guid therapistId, CancellationToken cancellationToken)
    {
        var patient = await dbContext.Patients
            .AsNoTracking()
            .FirstOrDefaultAsync(patient => patient.Id == PatientId.Of(patientId) && patient.TherapistId == therapistId, cancellationToken);

        if (patient == null)
        {
            throw new PatientNotFoundException(patientId);
        }

        var patientDto = patient.ToPatientDto();

        var sessions = await dbContext.Sessions
            .AsNoTracking()
            .Include(session => session.SessionGoals)
            .Where(session => session.PatientId == patientId && session.TherapistId == therapistId)
            .OrderBy(session => session.StartDateTime)
            .ToListAsync(cancellationToken);

        var goals = await dbContext.TherapeuticGoals
            .AsNoTracking()
            .Where(goal => goal.PatientId == patientId && goal.TherapistId == therapistId)
            .OrderBy(goal => goal.Area)
            .ThenBy(goal => goal.Description)
            .ToListAsync(cancellationToken);

        var dashboard = await dashboardBuilder.BuildAsync(patientId, therapistId, cancellationToken);
        var periodStart = sessions.Any() ? sessions.Min(session => session.StartDateTime) : DateTime.SpecifyKind(DateTime.UtcNow.Date, DateTimeKind.Utc);
        var periodEnd = sessions.Any() ? sessions.Max(session => session.EndDateTime) : DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
        var title = $"Therapy Progress Report - {patient.Name}";

        var patientSnapshot = BuildPatientSnapshot(patientDto);
        var executiveSummary = BuildExecutiveSummary(patientDto, dashboard, periodStart, periodEnd);
        var attendanceSummary = BuildAttendanceSummary(dashboard);
        var goalProgressSummary = BuildGoalProgressSummary(goals, dashboard);
        var sessionSummary = BuildSessionSummary(sessions, dashboard);
        var recommendations = BuildRecommendations(sessions, patientDto);

        return Report.CreateDraft(
            id: Guid.NewGuid(),
            patientId: patient.Id.Value,
            therapistId: therapistId,
            title: title,
            periodStart: periodStart,
            periodEnd: periodEnd,
            patientSnapshot: patientSnapshot,
            executiveSummary: executiveSummary,
            attendanceSummary: attendanceSummary,
            goalProgressSummary: goalProgressSummary,
            sessionSummary: sessionSummary,
            recommendations: recommendations,
            additionalNotes: null);
    }

    private static string BuildPatientSnapshot(PatientDto patient)
    {
        var ageText = patient.CalculatedAge > 0 ? $"{patient.CalculatedAge} years old" : "age unavailable";
        var caregiverText = string.IsNullOrWhiteSpace(patient.CaregiverName)
            ? "No caregiver registered"
            : $"Caregiver: {patient.CaregiverName}";

        return string.Join(Environment.NewLine, new[]
        {
            $"Patient: {patient.Name}",
            $"Age: {ageText}",
            $"Main diagnosis: {patient.MainDiagnosis}",
            $"Gender: {patient.Gender ?? "Not specified"}",
            $"Phone: {patient.PhoneNumber ?? "Not specified"}",
            $"Email: {patient.Email ?? "Not specified"}",
            caregiverText,
            $"Referral reason: {patient.ReferralReason ?? "Not specified"}",
            $"General notes: {patient.GeneralNotes ?? "None"}"
        });
    }

    private static string BuildExecutiveSummary(PatientDto patient, PatientProgressDashboardDto dashboard, DateTime periodStart, DateTime periodEnd)
    {
        return string.Join(Environment.NewLine, new[]
        {
            $"This report covers {patient.Name} from {periodStart:dd MMM yyyy} to {periodEnd:dd MMM yyyy}.",
            $"During this period, {dashboard.Attendance.CompletedSessions} sessions were completed out of {dashboard.Attendance.TotalSessions} total sessions.",
            $"The current goal completion rate is {dashboard.Goals.CompletionRate:0.#}% across {dashboard.Goals.TotalGoals} goals."
        });
    }

    private static string BuildAttendanceSummary(PatientProgressDashboardDto dashboard)
    {
        return string.Join(Environment.NewLine, new[]
        {
            $"Completed sessions: {dashboard.Attendance.CompletedSessions}",
            $"Cancelled sessions: {dashboard.Attendance.CancelledSessions}",
            $"No-show sessions: {dashboard.Attendance.NoShowSessions}",
            $"Upcoming sessions: {dashboard.Attendance.UpcomingSessions}",
            $"Pending registration sessions: {dashboard.Attendance.PendingRegistrationSessions}",
            $"Attendance rate: {dashboard.Attendance.AttendanceRate:0.#}%"
        });
    }

    private static string BuildGoalProgressSummary(IEnumerable<TherapeuticGoal> goals, PatientProgressDashboardDto dashboard)
    {
        var goalList = goals.Any()
            ? string.Join(", ", goals.Select(goal => $"{goal.Area} ({goal.Status})"))
            : "No goals recorded yet.";

        return string.Join(Environment.NewLine, new[]
        {
            $"Total goals: {dashboard.Goals.TotalGoals}",
            $"Not started: {dashboard.Goals.NotStartedGoals}",
            $"In progress: {dashboard.Goals.InProgressGoals}",
            $"Achieved: {dashboard.Goals.AchievedGoals}",
            $"Suspended: {dashboard.Goals.SuspendedGoals}",
            $"Completion rate: {dashboard.Goals.CompletionRate:0.#}%",
            $"Goal areas: {goalList}"
        });
    }

    private static string BuildSessionSummary(IEnumerable<Session> sessions, PatientProgressDashboardDto dashboard)
    {
        var lastSessionText = dashboard.LastSession == null
            ? "No completed or past session available."
            : $"Last session: {dashboard.LastSession.StartDateTime:dd MMM yyyy HH:mm} - {dashboard.LastSession.Type} - {dashboard.LastSession.Status}";

        var nextSessionText = dashboard.NextSession == null
            ? "No upcoming session scheduled."
            : $"Next session: {dashboard.NextSession.StartDateTime:dd MMM yyyy HH:mm} - {dashboard.NextSession.Type} - {dashboard.NextSession.Location}";

        var recentSessions = sessions
            .OrderByDescending(session => session.StartDateTime)
            .Take(5)
            .Select(session => $"{session.StartDateTime:dd MMM yyyy HH:mm} - {session.Type} - {session.Status}");

        return string.Join(Environment.NewLine, new[]
        {
            lastSessionText,
            nextSessionText,
            "Recent sessions:",
            string.Join(Environment.NewLine, recentSessions.DefaultIfEmpty("No sessions available.")),
            string.Empty
        });
    }

    private static string BuildRecommendations(IEnumerable<Session> sessions, PatientDto patient)
    {
        var latestRecommendation = sessions
            .OrderByDescending(session => session.StartDateTime)
            .Select(session => session.Recommendations)
            .FirstOrDefault(recommendation => !string.IsNullOrWhiteSpace(recommendation));

        if (!string.IsNullOrWhiteSpace(latestRecommendation))
        {
            return latestRecommendation!;
        }

        return string.Join(Environment.NewLine, new[]
        {
            $"Continue working on the current clinical plan for {patient.Name}.",
            "Review the active goals at the next scheduled session.",
            "Capture new checkpoints session by session so the next report can be generated faster."
        });
    }
}
