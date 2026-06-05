namespace Patients.Domain.Models;

public class TherapeuticGoal : Entity<Guid>
{
    public Guid PatientId { get; set; }
    public Guid TherapistId { get; set; }
    public Guid? ParentGoalId { get; set; }
    public string Description { get; set; } = default!;
    public string Area { get; set; } = default!;
    public TherapeuticGoalType Type { get; set; } = TherapeuticGoalType.ShortTerm;
    public TherapeuticGoalPriority Priority { get; set; } = TherapeuticGoalPriority.Medium;
    public TherapeuticGoalStatus Status { get; set; } = TherapeuticGoalStatus.NotStarted;
    public DateTime? ReviewDate { get; set; }
    public TherapeuticGoal? ParentGoal { get; set; }
    public ICollection<TherapeuticGoal> ChildGoals { get; set; } = [];

    public static TherapeuticGoal Create(
        Guid id,
        Guid patientId,
        Guid therapistId,
        TherapeuticGoalType type,
        Guid? parentGoalId,
        string description,
        string area,
        TherapeuticGoalPriority priority,
        DateTime? reviewDate)
    {
        return new TherapeuticGoal
        {
            Id = id,
            PatientId = patientId,
            TherapistId = therapistId,
            ParentGoalId = parentGoalId,
            Description = description,
            Area = area,
            Type = type,
            Priority = priority,
            Status = TherapeuticGoalStatus.NotStarted,
            ReviewDate = reviewDate
        };
    }

    public void Update(
        TherapeuticGoalType type,
        Guid? parentGoalId,
        string description,
        string area,
        TherapeuticGoalPriority priority,
        DateTime? reviewDate)
    {
        Type = type;
        ParentGoalId = parentGoalId;
        Description = description;
        Area = area;
        Priority = priority;
        ReviewDate = reviewDate;
    }

    public void UpdateStatus(TherapeuticGoalStatus status)
    {
        Status = status;
    }
}
