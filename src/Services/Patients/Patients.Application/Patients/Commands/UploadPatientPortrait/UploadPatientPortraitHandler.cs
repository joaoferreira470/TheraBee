namespace Patients.Application.Patients.Commands.UploadPatientPortrait;

public class UploadPatientPortraitHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUserService,
    IPortraitStorage portraitStorage)
    : ICommandHandler<UploadPatientPortraitCommand, UploadPatientPortraitResult>
{
    public async Task<UploadPatientPortraitResult> Handle(
        UploadPatientPortraitCommand command,
        CancellationToken cancellationToken)
    {
        var currentUserId = currentUserService.UserId
            ?? throw new UnauthorizedAccessException("User is not authenticated.");

        var patient = await dbContext.Patients.FirstOrDefaultAsync(
            item => item.Id == PatientId.Of(command.PatientId) && item.TherapistId == currentUserId,
            cancellationToken)
            ?? throw new PatientNotFoundException(command.PatientId);

        var previousStorageKey = patient.PortraitStorageKey;
        var storedPortrait = await portraitStorage.SaveAsync(
            command.Content,
            command.ContentType,
            "patients",
            command.PatientId,
            cancellationToken);

        var updatedAt = DateTime.UtcNow;
        patient.SetPortrait(storedPortrait.StorageKey, storedPortrait.ContentType, updatedAt);

        try
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            await portraitStorage.DeleteAsync(storedPortrait.StorageKey, cancellationToken);
            throw;
        }

        if (previousStorageKey is not null)
        {
            await portraitStorage.DeleteAsync(previousStorageKey, cancellationToken);
        }

        return new UploadPatientPortraitResult(new PortraitInfoDto(true, updatedAt));
    }
}
