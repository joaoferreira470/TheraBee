using Patients.Application.Reports.Commands.DeleteReport;

namespace Patients.API.Endpoints;

public class DeleteReport : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/reports/{reportId}", async (Guid reportId, ISender sender) =>
        {
            await sender.Send(new DeleteReportCommand(reportId));

            return Results.NoContent();
        })
        .RequireAuthorization()
        .WithName("DeleteReport")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Delete Report")
        .WithDescription("Deletes a report draft owned by the authenticated therapist.");
    }
}
