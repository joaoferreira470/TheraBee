using BuildingBlocks.CQRS;

namespace Patients.Application.Patients.Commands.CreatePatient;

public class CreatePatientHandler : ICommandHandler<CreatePatientCommand, CreatePatientResult>
{
    public Task<CreatePatientResult> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
