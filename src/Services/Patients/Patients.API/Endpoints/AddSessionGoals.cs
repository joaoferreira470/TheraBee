using Microsoft.AspNetCore.Mvc;
using Patients.Application.Sessions.Commands.AddSessionGoals;

namespace Patients.API.Endpoints;

public record AddSessionGoalsRequest(AddSessionGoalsDto Goals);
public record AddSessionGoalsResponse(bool IsSuccess);

public class AddSessionGoals : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/sessions/{sessionId}/goals", async (Guid sessionId, [FromBody] AddSessionGoalsRequest request, ISender sender) =>
        {
            var result = await sender.Send(new AddSessionGoalsCommand(sessionId, request.Goals));
            var response = result.Adapt<AddSessionGoalsResponse>();

            return Results.Ok(response);
        })
        .RequireAuthorization()
        .WithName("AddSessionGoals")
        .Produces<AddSessionGoalsResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Add Session Goals")
        .WithDescription("Associates therapeutic goals with a therapy session.");
    }
}
