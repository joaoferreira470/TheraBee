using Patients.Application.TherapyGoals.Queries.GetTherapeuticGoalById;

namespace Patients.API.Endpoints;

public record GetTherapeuticGoalByIdResponse(TherapeuticGoalDto TherapeuticGoal);

public class GetTherapeuticGoalById : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/therapy-goals/{goalId}", async (Guid goalId, ISender sender) =>
        {
            var result = await sender.Send(new GetTherapeuticGoalByIdQuery(goalId));
            var response = result.Adapt<GetTherapeuticGoalByIdResponse>();

            return Results.Ok(response);
        })
        .RequireAuthorization()
        .WithName("GetTherapeuticGoalById")
        .Produces<GetTherapeuticGoalByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Get Therapeutic Goal By Id")
        .WithDescription("Gets a therapeutic goal that belongs to the authenticated therapist.");
    }
}
