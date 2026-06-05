namespace Patients.Application.Therapists.Queries.GetCurrentTherapist;

public class GetCurrentTherapistHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUserService)
    : IQueryHandler<GetCurrentTherapistQuery, GetCurrentTherapistResult>
{
    public async Task<GetCurrentTherapistResult> Handle(GetCurrentTherapistQuery query, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId
            ?? throw new InvalidOperationException("Authenticated user is required.");

        var therapist = await dbContext.TherapistProfiles
            .AsNoTracking()
            .FirstOrDefaultAsync(profile => profile.UserId == userId, cancellationToken)
            ?? throw new InvalidOperationException("Therapist profile was not found.");

        return new GetCurrentTherapistResult(therapist.ToDto());
    }
}

