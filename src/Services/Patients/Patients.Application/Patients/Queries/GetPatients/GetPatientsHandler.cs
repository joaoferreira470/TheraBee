
using BuildingBlocks.Pagination;

namespace Patients.Application.Patients.Queries.GetPatients;

public class GetPatientsHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetPatientsQuery, GetPatientsResult>
{
    public async Task<GetPatientsResult> Handle(GetPatientsQuery query, CancellationToken cancellationToken)
    {
        var pageIndex = query.PaginationRequest.PageIndex;
        var pageSize = query.PaginationRequest.PageSize;

        var totalCount = await dbContext.Patients.LongCountAsync(cancellationToken);

        var patients = await dbContext.Patients
                                        .Skip(pageSize * pageIndex)
                                        .Take(pageSize)
                                        .ToListAsync(cancellationToken);

        var result = new PaginatedResult<PatientDto>(pageIndex, pageSize, totalCount, patients.ToPatientDtoList());

        return new GetPatientsResult(result);
    }
}
