using Patients.Application.Patients.Queries.GetPatientById;
using Patients.Domain.ValueObjects;

namespace Patients.API.Endpoints;

public record GetPatientByIdResponse(PatientDto Patient);
public class GetPatientById : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/patients/id/{id}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetPatientByIdQuery(PatientId.Of(id)));

            var response = result.Adapt<GetPatientByIdResponse>();

            return Results.Ok(response);
        })
        .WithName("GetPatientById")
        .Produces<GetPatientByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Get Patient By Id")
        .WithDescription("Get Patient By Id");
    }
}