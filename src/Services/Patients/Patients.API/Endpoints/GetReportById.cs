using Patients.Application.Reports.Queries.GetReportById;

namespace Patients.API.Endpoints;

public record GetReportByIdResponse(ReportDto Report);

public class GetReportById : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/reports/{reportId}", async (Guid reportId, ISender sender) =>
        {
            var result = await sender.Send(new GetReportByIdQuery(reportId));
            var response = result.Adapt<GetReportByIdResponse>();

            return Results.Ok(response);
        })
        .RequireAuthorization()
        .WithName("GetReportById")
        .Produces<GetReportByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Get Report By Id")
        .WithDescription("Gets a report by id for the authenticated therapist.");
    }
}
