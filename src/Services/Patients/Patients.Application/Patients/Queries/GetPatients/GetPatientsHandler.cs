using BuildingBlocks.Pagination;

namespace Patients.Application.Patients.Queries.GetPatients;

public class GetPatientsHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService)
    : IQueryHandler<GetPatientsQuery, GetPatientsResult>
{
    public async Task<GetPatientsResult> Handle(GetPatientsQuery query, CancellationToken cancellationToken)
    {
        var currentUserId = currentUserService.UserId
            ?? throw new UnauthorizedAccessException("User is not authenticated.");

        var pageIndex = query.PaginationRequest.PageIndex;
        var pageSize = query.PaginationRequest.PageSize;

        var patientQuery = dbContext.Patients
            .AsNoTracking()
            .Where(p => p.TherapistId == currentUserId);

        var totalCount = await patientQuery.LongCountAsync(cancellationToken);

        var patients = await patientQuery
            .OrderBy(p => p.Name)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var result = new PaginatedResult<PatientDto>(pageIndex, pageSize, totalCount, patients.ToPatientDtoList());

        return new GetPatientsResult(result);
    }
}
