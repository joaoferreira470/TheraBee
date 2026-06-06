using Microsoft.AspNetCore.Mvc;
using Patients.Application.Sessions.Commands.SetSessionGoalAssessments;

namespace Patients.API.Endpoints;

public record SetSessionGoalAssessmentsRequest(SetSessionGoalAssessmentsDto Assessments);
public record SetSessionGoalAssessmentsResponse(bool IsSuccess);

public class SetSessionGoalAssessments : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/sessions/{sessionId}/goal-assessments", async (Guid sessionId, [FromBody] SetSessionGoalAssessmentsRequest request, ISender sender) =>
        {
            var result = await sender.Send(new SetSessionGoalAssessmentsCommand(sessionId, request.Assessments));
            var response = result.Adapt<SetSessionGoalAssessmentsResponse>();

            return Results.Ok(response);
        })
        .RequireAuthorization()
        .WithName("SetSessionGoalAssessments")
        .Produces<SetSessionGoalAssessmentsResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Set Session Goal Assessments")
        .WithDescription("Creates or updates the 0-10 clinical assessments for the goals attached to a session.");
    }
}
