namespace Patients.Application.Dtos;

public record UpdateTherapeuticGoalDto(
    string Description,
    string Area,
    TherapeuticGoalPriority Priority,
    DateTime? ReviewDate);
