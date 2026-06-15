using Patients.Application.TherapyGoals.Commands.DeleteTherapeuticGoal;

namespace Patients.API.Endpoints;

public record DeleteTherapeuticGoalResponse(bool IsSuccess);

public class DeleteTherapeuticGoal : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/therapy-goals/{goalId}", async (Guid goalId, ISender sender) =>
        {
            var result = await sender.Send(new DeleteTherapeuticGoalCommand(goalId));
            var response = result.Adapt<DeleteTherapeuticGoalResponse>();

            return Results.Ok(response);
        })
        .RequireAuthorization()
        .WithName("DeleteTherapeuticGoal")
        .Produces<DeleteTherapeuticGoalResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Delete Therapeutic Goal")
        .WithDescription("Permanently deletes a therapeutic preset and its session associations.");
    }
}
