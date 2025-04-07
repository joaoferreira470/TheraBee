using BuildingBlocks.CQRS;
using FluentValidation;
using Patients.Application.Dtos;

namespace Patients.Application.Patients.Commands.UpdatePatient;

public record UpdatePatientCommand(PatientDto Patient)
    : ICommand<UpdatePatientResult>;

public record UpdatePatientResult(bool IsSuccess);

public class UpdatePatientCommandValidator : AbstractValidator<UpdatePatientCommand>
{
    public UpdatePatientCommandValidator()
    {
        RuleFor(x => x.Patient.Id).NotEmpty().WithMessage("Id is required");
    }
}
