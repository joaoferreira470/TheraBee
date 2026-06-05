using Microsoft.AspNetCore.Mvc;
using Patients.Application.Sessions.Commands.CancelSession;

namespace Patients.API.Endpoints;

public record CancelSessionRequest(CancelSessionDto Session);
public record CancelSessionResponse(bool IsSuccess);

public class CancelSession : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPatch("/sessions/{sessionId}/cancel", async (Guid sessionId, [FromBody] CancelSessionRequest request, ISender sender) =>
        {
            var result = await sender.Send(new CancelSessionCommand(sessionId, request.Session));
            var response = result.Adapt<CancelSessionResponse>();

            return Results.Ok(response);
        })
        .RequireAuthorization()
        .WithName("CancelSession")
        .Produces<CancelSessionResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Cancel Session")
        .WithDescription("Cancels an existing therapy session.");
    }
}
