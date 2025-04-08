using BuildingBlocks.Pagination;

namespace Patients.Application.Patients.Queries.GetPatients;

public record GetPatientsQuery(PaginatedRequest PaginationRequest)
    : IQuery<GetPatientsResult>;

public record GetPatientsResult(PaginatedResult<PatientDto> Patients);
