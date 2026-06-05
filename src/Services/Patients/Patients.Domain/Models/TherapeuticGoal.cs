namespace Patients.Domain.Models;

public class TherapeuticGoal : Entity<Guid>
{
    public Guid PatientId { get; set; }
    public Guid TherapistId { get; set; }
    public string Description { get; set; } = default!;
    public string Area { get; set; } = default!;
    public TherapeuticGoalPriority Priority { get; set; } = TherapeuticGoalPriority.Medium;
    public TherapeuticGoalStatus Status { get; set; } = TherapeuticGoalStatus.NotStarted;
    public DateTime? ReviewDate { get; set; }

    public static TherapeuticGoal Create(
        Guid id,
        Guid patientId,
        Guid therapistId,
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
            Description = description,
            Area = area,
            Priority = priority,
            Status = TherapeuticGoalStatus.NotStarted,
            ReviewDate = reviewDate
        };
    }

    public void Update(
        string description,
        string area,
        TherapeuticGoalPriority priority,
        DateTime? reviewDate)
    {
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
