using BuildingBlocks.CQRS;
using FluentValidation;
using Patients.Application.Dtos;

namespace Patients.Application.Patients.Commands.UpdatePatient;

public record UpdatePatientCommand(UpdatePatientDto Patient)
    : ICommand<UpdatePatientResult>;

public record UpdatePatientResult(bool IsSuccess);

public class UpdatePatientCommandValidator : AbstractValidator<UpdatePatientCommand>
{
    public UpdatePatientCommandValidator()
    {
        RuleFor(x => x.Patient.Id).NotEmpty().WithMessage("Id is required");
        RuleFor(x => x.Patient.Name).NotEmpty().MaximumLength(150).WithMessage("Name is required");
        RuleFor(x => x.Patient.DateOfBirth)
            .Must(date => date != default)
            .WithMessage("Date of birth is required");
        RuleFor(x => x.Patient.DateOfBirth)
            .LessThan(DateTime.UtcNow).WithMessage("Date of birth cannot be in the future");
        RuleFor(x => x.Patient.PatientAddress).NotEmpty().WithMessage("PatientAddress is required");
        RuleFor(x => x.Patient.MainDiagnosis).NotEmpty().MaximumLength(500).WithMessage("MainDiagnosis is required");
        RuleFor(x => x.Patient.Gender).MaximumLength(50);
        RuleFor(x => x.Patient.PhoneNumber).MaximumLength(40);
        RuleFor(x => x.Patient.Email)
            .EmailAddress()
            .When(x => !string.IsNullOrWhiteSpace(x.Patient.Email));
        RuleFor(x => x.Patient.CaregiverName).MaximumLength(150);
        RuleFor(x => x.Patient.CaregiverPhone).MaximumLength(40);
        RuleFor(x => x.Patient.ReferralReason).MaximumLength(500);
        RuleFor(x => x.Patient.GeneralNotes).MaximumLength(1000);
    }
}
