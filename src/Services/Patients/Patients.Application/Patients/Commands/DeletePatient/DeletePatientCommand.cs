using FluentValidation;
using Patients.Application.Patients.Commands.UpdatePatient;

namespace Patients.Application.Patients.Commands.DeletePatient;

public record DeletePatientCommand(Guid PatientId)
    : ICommand<DeletePatientResult>;

public record DeletePatientResult(bool IsSuccess);

public class DeletePatientCommandValidator : AbstractValidator<DeletePatientCommand>
{
    public DeletePatientCommandValidator()
    {
        RuleFor(x => x.PatientId).NotEmpty().WithMessage("Id is required");
    }
}