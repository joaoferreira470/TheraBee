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
        RuleFor(x => x.Patient.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Patient.DateOfBirth)
            .Must(date => date != default)
            .WithMessage("Date of birth is required");
        RuleFor(x => x.Patient.DateOfBirth)
            .LessThan(DateTime.UtcNow).WithMessage("Date of birth cannot be in the future");
        RuleFor(x => x.Patient.PatientAddress).NotEmpty().WithMessage("PatientAddress is required");
        RuleFor(x => x.Patient.Diagnosis).NotEmpty().WithMessage("Diagnosis is required");
        RuleFor(x => x.Patient.Info).NotEmpty().WithMessage("Info is required");
    }
}
