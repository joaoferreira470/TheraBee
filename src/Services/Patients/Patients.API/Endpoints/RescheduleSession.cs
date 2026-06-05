using Microsoft.AspNetCore.Mvc;
using Patients.Application.Sessions.Commands.RescheduleSession;

namespace Patients.API.Endpoints;

public record RescheduleSessionRequest(RescheduleSessionDto Session);
public record RescheduleSessionResponse(bool IsSuccess);

public class RescheduleSession : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPatch("/sessions/{sessionId}/reschedule", async (Guid sessionId, [FromBody] RescheduleSessionRequest request, ISender sender) =>
        {
            var result = await sender.Send(new RescheduleSessionCommand(sessionId, request.Session));
            var response = result.Adapt<RescheduleSessionResponse>();

            return Results.Ok(response);
        })
        .RequireAuthorization()
        .WithName("RescheduleSession")
        .Produces<RescheduleSessionResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Reschedule Session")
        .WithDescription("Reschedules an existing therapy session.");
    }
}
