using Patients.Application.Reports.Commands.ExportReportToPdf;

namespace Patients.API.Endpoints;

public class ExportReportToPdf : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/reports/{reportId}/export/pdf", async (Guid reportId, ISender sender) =>
        {
            var result = await sender.Send(new ExportReportToPdfCommand(reportId));
            return Results.File(result.Content, result.ContentType, result.FileName);
        })
        .RequireAuthorization()
        .WithName("ExportReportToPdf")
        .Produces(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Export Report To PDF")
        .WithDescription("Exports a report to PDF and stores the generated file in patient history.");
    }
}
