using Microsoft.AspNetCore.Mvc;
using Patients.Application.Sessions.Commands.CompleteSession;

namespace Patients.API.Endpoints;

public record CompleteSessionRequest(CompleteSessionDto Session);
public record CompleteSessionResponse(bool IsSuccess);

public class CompleteSession : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPatch("/sessions/{sessionId}/complete", async (Guid sessionId, [FromBody] CompleteSessionRequest request, ISender sender) =>
        {
            var result = await sender.Send(new CompleteSessionCommand(sessionId, request.Session));
            var response = result.Adapt<CompleteSessionResponse>();

            return Results.Ok(response);
        })
        .RequireAuthorization()
        .WithName("CompleteSession")
        .Produces<CompleteSessionResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Complete Session")
        .WithDescription("Registers the clinical notes for a completed therapy session.");
    }
}
