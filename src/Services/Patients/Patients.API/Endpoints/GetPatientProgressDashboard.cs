using Patients.Application.ProgressDashboards.Queries.GetPatientProgressDashboard;

namespace Patients.API.Endpoints;

public record GetPatientProgressDashboardResponse(PatientProgressDashboardDto Dashboard);

public class GetPatientProgressDashboard : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/patients/{patientId}/progress-dashboard", async (Guid patientId, ISender sender) =>
        {
            var result = await sender.Send(new GetPatientProgressDashboardQuery(patientId));
            var response = result.Adapt<GetPatientProgressDashboardResponse>();

            return Results.Ok(response);
        })
        .RequireAuthorization()
        .WithName("GetPatientProgressDashboard")
        .Produces<GetPatientProgressDashboardResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Get Patient Progress Dashboard")
        .WithDescription("Gets the patient progress dashboard for the authenticated therapist.");
    }
}
