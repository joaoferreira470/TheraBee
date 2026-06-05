using Patients.Application.Sessions.Queries.GetSessions;

namespace Patients.API.Endpoints;

public record GetSessionsResponse(IEnumerable<SessionDto> Sessions);

public class GetSessions : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/sessions", async (ISender sender) =>
        {
            var result = await sender.Send(new GetSessionsQuery());
            var response = result.Adapt<GetSessionsResponse>();

            return Results.Ok(response);
        })
        .RequireAuthorization()
        .WithName("GetSessions")
        .Produces<GetSessionsResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .WithSummary("Get Sessions")
        .WithDescription("Gets all therapy sessions for the authenticated therapist.");
    }
}
