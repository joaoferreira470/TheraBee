using Patients.Application.Reports.Commands.CreateReportDraft;

namespace Patients.API.Endpoints;

public record CreateReportDraftRequest(DateTime PeriodStart, DateTime PeriodEnd);
public record CreateReportDraftResponse(Guid Id);

public class CreateReportDraft : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/patients/{patientId}/reports/draft", async (Guid patientId, CreateReportDraftRequest request, ISender sender) =>
        {
            var result = await sender.Send(new CreateReportDraftCommand(patientId, request.PeriodStart, request.PeriodEnd));
            var response = result.Adapt<CreateReportDraftResponse>();

            return Results.Created($"/reports/{response.Id}", response);
        })
        .RequireAuthorization()
        .WithName("CreateReportDraft")
        .Produces<CreateReportDraftResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Create Report Draft")
        .WithDescription("Generates a report draft for the authenticated therapist's patient.");
    }
}
