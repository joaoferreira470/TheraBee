namespace Patients.Application.Patients.Queries.GetDuplicatePatients;

public class GetDuplicatePatientsQueryHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService)
    : IQueryHandler<GetDuplicatePatientsQuery, GetDuplicatePatientsResult>
{
    public async Task<GetDuplicatePatientsResult> Handle(GetDuplicatePatientsQuery query, CancellationToken cancellationToken)
    {
        var currentUserId = currentUserService.UserId
            ?? throw new UnauthorizedAccessException("User is not authenticated.");

        var normalizedName = query.Name.Trim().ToLower();
        var normalizedPhoneNumber = NormalizeOptional(query.PhoneNumber);
        var normalizedEmail = NormalizeOptional(query.Email);

        var patients = await dbContext.Patients
            .AsNoTracking()
            .Where(p => p.TherapistId == currentUserId
                && (
                    (p.Name.ToLower() == normalizedName
                        && p.DateOfBirth.Date == query.DateOfBirth.Date)
                    || (normalizedPhoneNumber != null
                        && p.PhoneNumber != null
                        && p.PhoneNumber.ToLower() == normalizedPhoneNumber)
                    || (normalizedEmail != null
                        && p.Email != null
                        && p.Email.ToLower() == normalizedEmail)
                ))
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);

        return new GetDuplicatePatientsResult(patients.ToPatientDtoList());
    }

    private static string? NormalizeOptional(string? value)
        => string.IsNullOrWhiteSpace(value) ? null : value.Trim().ToLower();
}
