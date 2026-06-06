namespace Patients.Domain.Models;

public class SessionGoalAssessment : Entity<Guid>
{
    public Guid SessionId { get; set; }
    public Guid TherapeuticGoalId { get; set; }
    public Guid TherapistId { get; set; }
    public int Score { get; set; }
    public string? ClinicalNotes { get; set; }

    public static SessionGoalAssessment Create(
        Guid id,
        Guid sessionId,
        Guid therapeuticGoalId,
        Guid therapistId,
        int score,
        string? clinicalNotes)
    {
        ValidateScore(score);

        return new SessionGoalAssessment
        {
            Id = id,
            SessionId = sessionId,
            TherapeuticGoalId = therapeuticGoalId,
            TherapistId = therapistId,
            Score = score,
            ClinicalNotes = clinicalNotes?.Trim()
        };
    }

    public void Update(int score, string? clinicalNotes)
    {
        ValidateScore(score);

        Score = score;
        ClinicalNotes = clinicalNotes?.Trim();
    }

    private static void ValidateScore(int score)
    {
        if (score is < 0 or > 10)
        {
            throw new InvalidOperationException("Score must be between 0 and 10.");
        }
    }
}
