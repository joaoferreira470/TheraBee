using Patients.Application.Patients.Commands.DeletePatient;

namespace Patients.API.Endpoints;

public record DeletePatientResponse(bool IsSuccess);

public class DeletePatient : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/patients/{id}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new DeletePatientCommand(id));
            var response = result.Adapt<DeletePatientResponse>();

            return Results.Ok(response);
        })
        .RequireAuthorization()
        .WithName("DeletePatient")
        .Produces<DeletePatientResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Delete Patient")
        .WithDescription("Permanently deletes the patient and their clinical records owned by the authenticated therapist.");
    }
}
