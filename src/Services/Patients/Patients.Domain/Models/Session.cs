namespace Patients.Domain.Models;

public class Session : Entity<Guid>
{
    public Guid PatientId { get; set; }
    public Guid TherapistId { get; set; }
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public SessionType Type { get; set; }
    public string Location { get; set; } = default!;
    public SessionStatus Status { get; set; } = SessionStatus.Scheduled;

    public string? CancellationReason { get; set; }
    public string? ClinicalSummary { get; set; }
    public string? ObjectivesWorked { get; set; }
    public string? ProgressRating { get; set; }
    public string? Activities { get; set; }
    public string? PatientResponse { get; set; }
    public string? Difficulties { get; set; }
    public string? Recommendations { get; set; }
    public string? NextSteps { get; set; }

    public ICollection<SessionGoal> SessionGoals { get; set; } = new List<SessionGoal>();

    public static Session Create(
        Guid id,
        Guid patientId,
        Guid therapistId,
        DateTime startDateTime,
        DateTime endDateTime,
        SessionType type,
        string location)
    {
        return new Session
        {
            Id = id,
            PatientId = patientId,
            TherapistId = therapistId,
            StartDateTime = startDateTime,
            EndDateTime = endDateTime,
            Type = type,
            Location = location.Trim(),
            Status = SessionStatus.Scheduled
        };
    }

    public void Reschedule(DateTime startDateTime, DateTime endDateTime, SessionType type, string location)
    {
        EnsureMutable();

        StartDateTime = startDateTime;
        EndDateTime = endDateTime;
        Type = type;
        Location = location.Trim();
        Status = SessionStatus.Rescheduled;
    }

    public void Cancel(string reason)
    {
        EnsureMutable();

        Status = SessionStatus.Cancelled;
        CancellationReason = reason.Trim();
    }

    public void MarkAsNoShow(SessionStatus status, string reason)
    {
        EnsureMutable();

        if (status is not SessionStatus.PatientNoShow and not SessionStatus.TherapistNoShow)
        {
            throw new InvalidOperationException("Only no-show statuses are allowed.");
        }

        Status = status;
        CancellationReason = reason.Trim();
    }

    public void Complete(
        string clinicalSummary,
        string objectivesWorked,
        string progressRating,
        string activities,
        string patientResponse,
        string difficulties,
        string recommendations,
        string nextSteps)
    {
        EnsureMutable();

        Status = SessionStatus.Completed;
        ClinicalSummary = clinicalSummary.Trim();
        ObjectivesWorked = objectivesWorked.Trim();
        ProgressRating = progressRating.Trim();
        Activities = activities.Trim();
        PatientResponse = patientResponse.Trim();
        Difficulties = difficulties.Trim();
        Recommendations = recommendations.Trim();
        NextSteps = nextSteps.Trim();
    }

    public void AddGoal(SessionGoal sessionGoal)
    {
        if (SessionGoals.Any(goal => goal.TherapeuticGoalId == sessionGoal.TherapeuticGoalId))
        {
            return;
        }

        SessionGoals.Add(sessionGoal);
    }

    private void EnsureMutable()
    {
        if (Status is SessionStatus.Completed or SessionStatus.Cancelled)
        {
            throw new InvalidOperationException("Completed or cancelled sessions cannot be modified.");
        }
    }
}
