using Patients.Application.Patients.Commands.UpdatePatientStatus;
using Patients.Domain.Models;

namespace Patients.API.Endpoints;

public record UpdatePatientStatusRequest(PatientStatus Status);

public record UpdatePatientStatusResponse(bool IsSuccess);

public class UpdatePatientStatus : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPatch("/patients/{id}/status", async (Guid id, UpdatePatientStatusRequest request, ISender sender) =>
        {
            var command = new UpdatePatientStatusCommand(id, request.Status);
            var result = await sender.Send(command);
            var response = result.Adapt<UpdatePatientStatusResponse>();

            return Results.Ok(response);
        })
        .RequireAuthorization()
        .WithName("UpdatePatientStatus")
        .Produces<UpdatePatientStatusResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Update Patient Status")
        .WithDescription("Updates a patient's clinical status.");
    }
}
