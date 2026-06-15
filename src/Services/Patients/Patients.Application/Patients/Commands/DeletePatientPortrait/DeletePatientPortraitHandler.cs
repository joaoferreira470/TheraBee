namespace Patients.Application.Patients.Commands.DeletePatientPortrait;

public class DeletePatientPortraitHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUserService,
    IPortraitStorage portraitStorage)
    : ICommandHandler<DeletePatientPortraitCommand, DeletePatientPortraitResult>
{
    public async Task<DeletePatientPortraitResult> Handle(
        DeletePatientPortraitCommand command,
        CancellationToken cancellationToken)
    {
        var currentUserId = currentUserService.UserId
            ?? throw new UnauthorizedAccessException("User is not authenticated.");

        var patient = await dbContext.Patients.FirstOrDefaultAsync(
            item => item.Id == PatientId.Of(command.PatientId) && item.TherapistId == currentUserId,
            cancellationToken)
            ?? throw new PatientNotFoundException(command.PatientId);

        var storageKey = patient.PortraitStorageKey;
        patient.RemovePortrait();
        await dbContext.SaveChangesAsync(cancellationToken);

        if (storageKey is not null)
        {
            await portraitStorage.DeleteAsync(storageKey, cancellationToken);
        }

        return new DeletePatientPortraitResult(new PortraitInfoDto(false, null));
    }
}
