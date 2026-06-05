namespace Patients.Application.Patients.Queries.GetDuplicatePatients;

public class GetDuplicatePatientsQueryHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService)
    : IQueryHandler<GetDuplicatePatientsQuery, GetDuplicatePatientsResult>
{
    public async Task<GetDuplicatePatientsResult> Handle(GetDuplicatePatientsQuery query, CancellationToken cancellationToken)
    {
        var currentUserId = currentUserService.UserId
            ?? throw new UnauthorizedAccessException("User is not authenticated.");

        var normalizedName = query.Name.Trim().ToLower();

        var patients = await dbContext.Patients
            .AsNoTracking()
            .Where(p => p.TherapistId == currentUserId
                && p.Name.ToLower() == normalizedName
                && p.DateOfBirth.Date == query.DateOfBirth.Date)
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);

        return new GetDuplicatePatientsResult(patients.ToPatientDtoList());
    }
}
