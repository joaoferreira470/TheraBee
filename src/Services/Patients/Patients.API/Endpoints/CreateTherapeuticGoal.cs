using Microsoft.AspNetCore.Mvc;
using Patients.Application.TherapyGoals.Commands.CreateTherapeuticGoal;

namespace Patients.API.Endpoints;

public record CreateTherapeuticGoalRequest(CreateTherapeuticGoalDto Goal);
public record CreateTherapeuticGoalResponse(Guid Id);

public class CreateTherapeuticGoal : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/patients/{patientId}/therapy-goals", async (Guid patientId, [FromBody] CreateTherapeuticGoalRequest request, ISender sender) =>
        {
            var result = await sender.Send(new CreateTherapeuticGoalCommand(patientId, request.Goal));
            var response = result.Adapt<CreateTherapeuticGoalResponse>();

            return Results.Created($"/therapy-goals/{response.Id}", response);
        })
        .RequireAuthorization()
        .WithName("CreateTherapeuticGoal")
        .Produces<CreateTherapeuticGoalResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Create Therapeutic Goal")
        .WithDescription("Creates a therapeutic goal for an authenticated therapist's patient.");
    }
}
