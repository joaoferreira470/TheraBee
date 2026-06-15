namespace Patients.Application.Therapists.Commands.DeleteCurrentTherapistPortrait;

public class DeleteCurrentTherapistPortraitHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUserService,
    IPortraitStorage portraitStorage)
    : ICommandHandler<DeleteCurrentTherapistPortraitCommand, DeleteCurrentTherapistPortraitResult>
{
    public async Task<DeleteCurrentTherapistPortraitResult> Handle(
        DeleteCurrentTherapistPortraitCommand command,
        CancellationToken cancellationToken)
    {
        var currentUserId = currentUserService.UserId
            ?? throw new UnauthorizedAccessException("User is not authenticated.");

        var therapist = await dbContext.TherapistProfiles.FirstOrDefaultAsync(
            profile => profile.UserId == currentUserId,
            cancellationToken)
            ?? throw new InvalidOperationException("Therapist profile was not found.");

        var storageKey = therapist.PortraitStorageKey;
        therapist.RemovePortrait();
        await dbContext.SaveChangesAsync(cancellationToken);

        if (storageKey is not null)
        {
            await portraitStorage.DeleteAsync(storageKey, cancellationToken);
        }

        return new DeleteCurrentTherapistPortraitResult(new PortraitInfoDto(false, null));
    }
}
