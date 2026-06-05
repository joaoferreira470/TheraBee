namespace Patients.Application.Patients.Queries.GetPatientsByTherapist;

public class GetPatientsByTherapistQueryHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService)
    : IQueryHandler<GetPatientsByTherapistQuery, GetPatientsByTherapistResult>
{
    public async Task<GetPatientsByTherapistResult> Handle(GetPatientsByTherapistQuery query, CancellationToken cancellationToken)
    {
        var currentUserId = currentUserService.UserId
            ?? throw new UnauthorizedAccessException("User is not authenticated.");

        var patients = await dbContext.Patients
            .AsNoTracking()
            .Where(p => p.TherapistId == currentUserId)
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);

        return new GetPatientsByTherapistResult(patients.ToPatientDtoList());
    }
}
