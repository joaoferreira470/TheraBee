namespace Patients.Domain.Models;

public class SessionGoal : Entity<Guid>
{
    public Guid SessionId { get; set; }
    public Guid TherapeuticGoalId { get; set; }
    public Guid TherapistId { get; set; }

    public static SessionGoal Create(Guid id, Guid sessionId, Guid therapeuticGoalId, Guid therapistId)
    {
        return new SessionGoal
        {
            Id = id,
            SessionId = sessionId,
            TherapeuticGoalId = therapeuticGoalId,
            TherapistId = therapistId
        };
    }
}
