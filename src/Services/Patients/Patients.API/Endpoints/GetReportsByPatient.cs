using Patients.Application.Reports.Queries.GetReportsByPatient;

namespace Patients.API.Endpoints;

public record GetReportsByPatientResponse(IEnumerable<ReportDto> Reports);

public class GetReportsByPatient : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/patients/{patientId}/reports", async (Guid patientId, ISender sender) =>
        {
            var result = await sender.Send(new GetReportsByPatientQuery(patientId));
            var response = result.Adapt<GetReportsByPatientResponse>();

            return Results.Ok(response);
        })
        .RequireAuthorization()
        .WithName("GetReportsByPatient")
        .Produces<GetReportsByPatientResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Get Reports By Patient")
        .WithDescription("Gets the report history for a patient owned by the authenticated therapist.");
    }
}
