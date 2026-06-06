using Patients.Application.Patients.Queries.GetPatientGoalAssessments;

namespace Patients.API.Endpoints;

public record GetPatientGoalAssessmentsResponse(IEnumerable<SessionGoalAssessmentDto> Assessments);

public class GetPatientGoalAssessments : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/patients/{patientId}/goal-assessments", async (Guid patientId, ISender sender) =>
        {
            var result = await sender.Send(new GetPatientGoalAssessmentsQuery(patientId));
            var response = result.Adapt<GetPatientGoalAssessmentsResponse>();

            return Results.Ok(response);
        })
        .RequireAuthorization()
        .WithName("GetPatientGoalAssessments")
        .Produces<GetPatientGoalAssessmentsResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Get Patient Goal Assessments")
        .WithDescription("Gets all goal assessments for the authenticated therapist's patient.");
    }
}
