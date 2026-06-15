namespace Patients.Application.Therapists.Queries.GetCurrentTherapistPortrait;

public class GetCurrentTherapistPortraitHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUserService,
    IPortraitStorage portraitStorage)
    : IQueryHandler<GetCurrentTherapistPortraitQuery, GetCurrentTherapistPortraitResult>
{
    public async Task<GetCurrentTherapistPortraitResult> Handle(
        GetCurrentTherapistPortraitQuery query,
        CancellationToken cancellationToken)
    {
        var currentUserId = currentUserService.UserId
            ?? throw new UnauthorizedAccessException("User is not authenticated.");

        var therapist = await dbContext.TherapistProfiles.AsNoTracking().FirstOrDefaultAsync(
            profile => profile.UserId == currentUserId,
            cancellationToken)
            ?? throw new InvalidOperationException("Therapist profile was not found.");

        if (therapist.PortraitStorageKey is null)
        {
            throw new PortraitNotFoundException("Therapist");
        }

        var portrait = await portraitStorage.GetAsync(therapist.PortraitStorageKey, cancellationToken)
            ?? throw new PortraitNotFoundException("Therapist");

        return new GetCurrentTherapistPortraitResult(portrait);
    }
}
