using BuildingBlocks.CQRS;
using FluentValidation;
using Patients.Application.Dtos;

namespace Patients.Application.Patients.Commands.CreatePatient;

public record CreatePatientCommand(PatientDto Patient)
    : ICommand<CreatePatientResult>;

public record CreatePatientResult(Guid Id);

public class CreatePatientValidator : AbstractValidator<CreatePatientCommand>
{
    public CreatePatientValidator()
    {
        RuleFor(x => x.Patient.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Patient.DateOfBirth)
                    .Must(date => date != default)
                    .WithMessage("Date of birth is required");

        RuleFor(x => x.Patient.DateOfBirth)
                    .LessThan(DateTime.UtcNow).WithMessage("Date of birth cannot be in the future");


        RuleFor(x => x.Patient.PatientAddress).NotEmpty().WithMessage("PatientAddress is required");

        RuleFor(x => x.Patient.TherapistId)
                    .NotEqual(Guid.Empty).WithMessage("TherapistId is required");

        //verificar as validações da morada
    }
}
