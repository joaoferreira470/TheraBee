namespace Patients.Application.Therapists.Commands.UploadCurrentTherapistPortrait;

public class UploadCurrentTherapistPortraitHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUserService,
    IPortraitStorage portraitStorage)
    : ICommandHandler<UploadCurrentTherapistPortraitCommand, UploadCurrentTherapistPortraitResult>
{
    public async Task<UploadCurrentTherapistPortraitResult> Handle(
        UploadCurrentTherapistPortraitCommand command,
        CancellationToken cancellationToken)
    {
        var currentUserId = currentUserService.UserId
            ?? throw new UnauthorizedAccessException("User is not authenticated.");

        var therapist = await dbContext.TherapistProfiles.FirstOrDefaultAsync(
            profile => profile.UserId == currentUserId,
            cancellationToken)
            ?? throw new InvalidOperationException("Therapist profile was not found.");

        var previousStorageKey = therapist.PortraitStorageKey;
        var storedPortrait = await portraitStorage.SaveAsync(
            command.Content,
            command.ContentType,
            "therapists",
            currentUserId,
            cancellationToken);

        var updatedAt = DateTime.UtcNow;
        therapist.SetPortrait(storedPortrait.StorageKey, storedPortrait.ContentType, updatedAt);

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

        return new UploadCurrentTherapistPortraitResult(new PortraitInfoDto(true, updatedAt));
    }
}
