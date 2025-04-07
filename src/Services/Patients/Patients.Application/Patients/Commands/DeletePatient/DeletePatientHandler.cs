
namespace Patients.Application.Patients.Commands.DeletePatient;

public class DeletePatientHandler(IApplicationDbContext dbContext) : ICommandHandler<DeletePatientCommand, DeletePatientResult>
{
    public async Task<DeletePatientResult> Handle(DeletePatientCommand command, CancellationToken cancellationToken)
    {
        var patientId = PatientId.Of(command.PatientId);

        var patient = await dbContext.Patients.FindAsync([patientId], cancellationToken);

        if (patient == null)
        {
            throw new PatientNotFoundException(command.PatientId);
        }

        dbContext.Patients.Remove(patient);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new DeletePatientResult(true);
    }
}
