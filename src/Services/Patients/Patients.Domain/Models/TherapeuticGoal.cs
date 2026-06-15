namespace Patients.Domain.Models;

public class TherapeuticGoal : Entity<Guid>
{
    public Guid PatientId { get; set; }
    public Guid TherapistId { get; set; }
    public string Description { get; set; } = default!;
    public TherapeuticGoalType Type { get; set; } = TherapeuticGoalType.Objective;
    public TherapeuticGoalPriority Priority { get; set; } = TherapeuticGoalPriority.Medium;

    public static TherapeuticGoal Create(
        Guid id,
        Guid patientId,
        Guid therapistId,
        TherapeuticGoalType type,
        string description,
        TherapeuticGoalPriority priority)
    {
        return new TherapeuticGoal
        {
            Id = id,
            PatientId = patientId,
            TherapistId = therapistId,
            Description = description,
            Type = type,
            Priority = priority
        };
    }

    public void Update(
        TherapeuticGoalType type,
        string description,
        TherapeuticGoalPriority priority)
    {
        Type = type;
        Description = description;
        Priority = priority;
    }

}
