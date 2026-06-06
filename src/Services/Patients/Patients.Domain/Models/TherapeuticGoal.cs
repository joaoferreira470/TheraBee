namespace Patients.Domain.Models;

public class TherapeuticGoal : Entity<Guid>
{
    public Guid PatientId { get; set; }
    public Guid TherapistId { get; set; }
    public string Description { get; set; } = default!;
    public string Area { get; set; } = default!;
    public TherapeuticGoalType Type { get; set; } = TherapeuticGoalType.Objective;
    public TherapeuticGoalPriority Priority { get; set; } = TherapeuticGoalPriority.Medium;
    public TherapeuticGoalStatus Status { get; set; } = TherapeuticGoalStatus.NotStarted;

    public static TherapeuticGoal Create(
        Guid id,
        Guid patientId,
        Guid therapistId,
        TherapeuticGoalType type,
        string description,
        string area,
        TherapeuticGoalPriority priority)
    {
        return new TherapeuticGoal
        {
            Id = id,
            PatientId = patientId,
            TherapistId = therapistId,
            Description = description,
            Area = area,
            Type = type,
            Priority = priority,
            Status = TherapeuticGoalStatus.NotStarted
        };
    }

    public void Update(
        TherapeuticGoalType type,
        string description,
        string area,
        TherapeuticGoalPriority priority)
    {
        Type = type;
        Description = description;
        Area = area;
        Priority = priority;
    }

    public void UpdateStatus(TherapeuticGoalStatus status)
    {
        Status = status;
    }
}
