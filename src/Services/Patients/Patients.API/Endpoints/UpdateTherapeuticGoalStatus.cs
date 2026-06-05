using Patients.Domain.Models;
using Patients.Application.TherapyGoals.Commands.UpdateTherapeuticGoalStatus;

namespace Patients.API.Endpoints;

public record UpdateTherapeuticGoalStatusRequest(TherapeuticGoalStatus Status);
public record UpdateTherapeuticGoalStatusResponse(bool IsSuccess);

public class UpdateTherapeuticGoalStatus : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPatch("/therapy-goals/{goalId}/status", async (Guid goalId, UpdateTherapeuticGoalStatusRequest request, ISender sender) =>
        {
            var result = await sender.Send(new UpdateTherapeuticGoalStatusCommand(goalId, request.Status));
            var response = result.Adapt<UpdateTherapeuticGoalStatusResponse>();

            return Results.Ok(response);
        })
        .RequireAuthorization()
        .WithName("UpdateTherapeuticGoalStatus")
        .Produces<UpdateTherapeuticGoalStatusResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Update Therapeutic Goal Status")
        .WithDescription("Updates the status of an authenticated therapist's therapeutic goal.");
    }
}
