namespace Patients.Application.TherapyGoals.Queries.GetTherapeuticGoalsByPatient;

public record GetTherapeuticGoalsByPatientQuery(Guid PatientId)
    : IQuery<GetTherapeuticGoalsByPatientResult>;

public record GetTherapeuticGoalsByPatientResult(IEnumerable<TherapeuticGoalDto> TherapeuticGoals);
