using Patients.Application.Sessions.Queries.GetSessionById;

namespace Patients.API.Endpoints;

public record GetSessionByIdResponse(SessionDto Session);

public class GetSessionById : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/sessions/{sessionId}", async (Guid sessionId, ISender sender) =>
        {
            var result = await sender.Send(new GetSessionByIdQuery(sessionId));
            var response = result.Adapt<GetSessionByIdResponse>();

            return Results.Ok(response);
        })
        .RequireAuthorization()
        .WithName("GetSessionById")
        .Produces<GetSessionByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Get Session By Id")
        .WithDescription("Gets a therapy session by id for the authenticated therapist.");
    }
}
