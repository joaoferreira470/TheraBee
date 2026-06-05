using FluentValidation;

namespace Patients.Application.Therapists.Commands.UpdateCurrentTherapist;

public record UpdateCurrentTherapistCommand(
    string ProfessionalName,
    string Profession,
    string? Specialties,
    string? ProfessionalNumber,
    string? PhoneNumber,
    string? Workplace,
    string? ReportSignature)
    : ICommand<UpdateCurrentTherapistResult>;

public record UpdateCurrentTherapistResult(TherapistProfileDto Therapist);

public class UpdateCurrentTherapistCommandValidator : AbstractValidator<UpdateCurrentTherapistCommand>
{
    public UpdateCurrentTherapistCommandValidator()
    {
        RuleFor(x => x.ProfessionalName).NotEmpty().WithMessage("Professional name is required");
        RuleFor(x => x.Profession).NotEmpty().WithMessage("Profession is required");
    }
}

