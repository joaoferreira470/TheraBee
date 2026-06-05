namespace Patients.Application.Patients.Commands.UpdatePatientStatus;

public class UpdatePatientStatusHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService)
    : ICommandHandler<UpdatePatientStatusCommand, UpdatePatientStatusResult>
{
    public async Task<UpdatePatientStatusResult> Handle(UpdatePatientStatusCommand command, CancellationToken cancellationToken)
    {
        var currentUserId = currentUserService.UserId
            ?? throw new UnauthorizedAccessException("User is not authenticated.");

        var patientId = PatientId.Of(command.PatientId);

        var patient = await dbContext.Patients
            .FirstOrDefaultAsync(p => p.Id == patientId && p.TherapistId == currentUserId, cancellationToken);

        if (patient == null)
        {
            throw new PatientNotFoundException(command.PatientId);
        }

        patient.UpdateStatus(command.Status);

        dbContext.Patients.Update(patient);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new UpdatePatientStatusResult(true);
    }
}
