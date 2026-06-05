namespace Patients.Application.Extensions;

public static class SessionExtensions
{
    public static SessionDto ToSessionDto(this Session session)
    {
        return new SessionDto(
            Id: session.Id,
            PatientId: session.PatientId,
            TherapistId: session.TherapistId,
            StartDateTime: session.StartDateTime,
            EndDateTime: session.EndDateTime,
            Type: session.Type,
            Location: session.Location,
            Status: session.Status,
            CancellationReason: session.CancellationReason,
            ClinicalSummary: session.ClinicalSummary,
            ObjectivesWorked: session.ObjectivesWorked,
            ProgressRating: session.ProgressRating,
            Activities: session.Activities,
            PatientResponse: session.PatientResponse,
            Difficulties: session.Difficulties,
            Recommendations: session.Recommendations,
            NextSteps: session.NextSteps,
            GoalIds: session.SessionGoals.Select(goal => goal.TherapeuticGoalId),
            CreatedAt: session.CreatedAt);
    }

    public static IEnumerable<SessionDto> ToSessionDtoList(this IEnumerable<Session> sessions)
    {
        return sessions.Select(ToSessionDto);
    }
}
