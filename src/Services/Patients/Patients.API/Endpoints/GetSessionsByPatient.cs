using Patients.Application.Sessions.Queries.GetSessionsByPatient;

namespace Patients.API.Endpoints;

public record GetSessionsByPatientResponse(IEnumerable<SessionDto> Sessions);

public class GetSessionsByPatient : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/patients/{patientId}/sessions", async (Guid patientId, ISender sender) =>
        {
            var result = await sender.Send(new GetSessionsByPatientQuery(patientId));
            var response = result.Adapt<GetSessionsByPatientResponse>();

            return Results.Ok(response);
        })
        .RequireAuthorization()
        .WithName("GetSessionsByPatient")
        .Produces<GetSessionsByPatientResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Get Sessions By Patient")
        .WithDescription("Gets all sessions for a patient owned by the authenticated therapist.");
    }
}
