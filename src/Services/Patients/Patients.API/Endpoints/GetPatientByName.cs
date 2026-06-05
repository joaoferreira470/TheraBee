using Patients.Application.Patients.Queries.GetPatientByName;

namespace Patients.API.Endpoints;

public record GetPatientByNameResponse(PatientDto Patient);

public class GetPatientByName : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/patients/name/{patientName}", async (string patientName, ISender sender) =>
        {
            var result = await sender.Send(new GetPatientByNameQuery(patientName));
            var response = result.Adapt<GetPatientByNameResponse>();

            return Results.Ok(response);
        })
        .RequireAuthorization()
        .WithName("GetPatientByName")
        .Produces<GetPatientByNameResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Get Patient By Name")
        .WithDescription("Gets a patient by name for the authenticated therapist.");
    }
}
