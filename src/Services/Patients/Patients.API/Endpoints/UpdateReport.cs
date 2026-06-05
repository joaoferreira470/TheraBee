using Microsoft.AspNetCore.Mvc;
using Patients.Application.Reports.Commands.UpdateReport;

namespace Patients.API.Endpoints;

public record UpdateReportRequest(UpdateReportDto Report);
public record UpdateReportResponse(bool IsSuccess);

public class UpdateReport : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/reports/{reportId}", async (Guid reportId, [FromBody] UpdateReportRequest request, ISender sender) =>
        {
            var result = await sender.Send(new UpdateReportCommand(reportId, request.Report));
            var response = result.Adapt<UpdateReportResponse>();

            return Results.Ok(response);
        })
        .RequireAuthorization()
        .WithName("UpdateReport")
        .Produces<UpdateReportResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Update Report")
        .WithDescription("Updates the editable sections of a report before export.");
    }
}
