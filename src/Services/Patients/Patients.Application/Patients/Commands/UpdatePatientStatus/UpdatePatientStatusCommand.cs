using BuildingBlocks.CQRS;
using FluentValidation;

namespace Patients.Application.Patients.Commands.UpdatePatientStatus;

public record UpdatePatientStatusCommand(Guid PatientId, PatientStatus Status)
    : ICommand<UpdatePatientStatusResult>;

public record UpdatePatientStatusResult(bool IsSuccess);

public class UpdatePatientStatusCommandValidator : AbstractValidator<UpdatePatientStatusCommand>
{
    public UpdatePatientStatusCommandValidator()
    {
        RuleFor(x => x.PatientId).NotEmpty().WithMessage("Id is required");
    }
}
