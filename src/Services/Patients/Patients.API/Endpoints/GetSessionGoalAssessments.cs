using Patients.Application.Sessions.Queries.GetSessionGoalAssessments;

namespace Patients.API.Endpoints;

public record GetSessionGoalAssessmentsResponse(IEnumerable<SessionGoalAssessmentDto> Assessments);

public class GetSessionGoalAssessments : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/sessions/{sessionId}/goal-assessments", async (Guid sessionId, ISender sender) =>
        {
            var result = await sender.Send(new GetSessionGoalAssessmentsQuery(sessionId));
            var response = result.Adapt<GetSessionGoalAssessmentsResponse>();

            return Results.Ok(response);
        })
        .RequireAuthorization()
        .WithName("GetSessionGoalAssessments")
        .Produces<GetSessionGoalAssessmentsResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Get Session Goal Assessments")
        .WithDescription("Gets the goal assessments attached to a session for the authenticated therapist.");
    }
}
