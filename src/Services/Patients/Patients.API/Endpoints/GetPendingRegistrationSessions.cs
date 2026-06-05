using Patients.Application.Sessions.Queries.GetPendingRegistrationSessions;

namespace Patients.API.Endpoints;

public record GetPendingRegistrationSessionsResponse(IEnumerable<SessionDto> Sessions);

public class GetPendingRegistrationSessions : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/sessions/pending-registration", async (ISender sender) =>
        {
            var result = await sender.Send(new GetPendingRegistrationSessionsQuery());
            var response = result.Adapt<GetPendingRegistrationSessionsResponse>();

            return Results.Ok(response);
        })
        .RequireAuthorization()
        .WithName("GetPendingRegistrationSessions")
        .Produces<GetPendingRegistrationSessionsResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .WithSummary("Get Pending Registration Sessions")
        .WithDescription("Gets sessions that are pending clinical registration for the authenticated therapist.");
    }
}
