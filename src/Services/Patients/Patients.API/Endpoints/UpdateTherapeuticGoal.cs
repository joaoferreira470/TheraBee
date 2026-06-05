using Patients.Application.TherapyGoals.Commands.UpdateTherapeuticGoal;

namespace Patients.API.Endpoints;

public record UpdateTherapeuticGoalRequest(UpdateTherapeuticGoalDto Goal);
public record UpdateTherapeuticGoalResponse(bool IsSuccess);

public class UpdateTherapeuticGoal : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/therapy-goals/{goalId}", async (Guid goalId, UpdateTherapeuticGoalRequest request, ISender sender) =>
        {
            var result = await sender.Send(new UpdateTherapeuticGoalCommand(goalId, request.Goal));
            var response = result.Adapt<UpdateTherapeuticGoalResponse>();

            return Results.Ok(response);
        })
        .RequireAuthorization()
        .WithName("UpdateTherapeuticGoal")
        .Produces<UpdateTherapeuticGoalResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Update Therapeutic Goal")
        .WithDescription("Updates an authenticated therapist's therapeutic goal.");
    }
}
