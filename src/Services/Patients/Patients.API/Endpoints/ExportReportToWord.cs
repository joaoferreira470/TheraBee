using Patients.Application.Reports.Commands.ExportReportToWord;

namespace Patients.API.Endpoints;

public class ExportReportToWord : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/reports/{reportId}/export/word", async (Guid reportId, ISender sender) =>
        {
            var result = await sender.Send(new ExportReportToWordCommand(reportId));
            return Results.File(result.Content, result.ContentType, result.FileName);
        })
        .RequireAuthorization()
        .WithName("ExportReportToWord")
        .Produces(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Export Report To Word")
        .WithDescription("Exports a report to Word and stores the generated file in patient history.");
    }
}
