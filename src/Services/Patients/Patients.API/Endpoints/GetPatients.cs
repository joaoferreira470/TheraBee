using BuildingBlocks.Pagination;
using Patients.Application.Patients.Queries.GetPatients;

namespace Patients.API.Endpoints;

public record GetPatientsResponse(PaginatedResult<PatientDto> Patients);
public class GetPatients : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/patients", async ([AsParameters] PaginatedRequest request, ISender sender) =>
        {
            var result = await sender.Send(new GetPatientsQuery(request));

            var response = result.Adapt<GetPatientsResponse>();

            return Results.Ok(response);
        })
        .WithName("GetPatients")
        .Produces<GetPatientsResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Get Patients")
        .WithDescription("Get Patients");
    }
}
