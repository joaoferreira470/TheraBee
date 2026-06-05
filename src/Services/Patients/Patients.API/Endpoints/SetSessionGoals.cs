using Microsoft.AspNetCore.Mvc;
using Patients.Application.Sessions.Commands.SetSessionGoals;

namespace Patients.API.Endpoints;

public record SetSessionGoalsRequest(SetSessionGoalsDto Goals);
public record SetSessionGoalsResponse(bool IsSuccess);

public class SetSessionGoals : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPatch("/sessions/{sessionId}/goals", async (Guid sessionId, [FromBody] SetSessionGoalsRequest request, ISender sender) =>
        {
            var result = await sender.Send(new SetSessionGoalsCommand(sessionId, request.Goals));
            var response = result.Adapt<SetSessionGoalsResponse>();
            return Results.Ok(response);
        })
        .RequireAuthorization()
        .WithName("SetSessionGoals")
        .Produces<SetSessionGoalsResponse>(StatusCodes.Status200OK)
        .WithSummary("Replaces the goals attached to a session.")
        .WithDescription("Replaces the existing session goals with the selected clinical goals.");
    }
}
