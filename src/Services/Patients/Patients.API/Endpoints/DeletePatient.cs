using Patients.Application.Patients.Commands.DeletePatient;
using Patients.Application.Patients.Commands.UpdatePatient;

namespace Patients.API.Endpoints;

public record DeletePatientRequest(Guid Id);

public record DeletePatientResponse(bool IsSuccess);
public class DeletePatient : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/patients/{id}", async (Guid Id, ISender sender) =>
        {
            var result = await sender.Send(new DeletePatientCommand(Id));

            var response = result.Adapt<DeletePatientResponse>();

            return Results.Ok(response);
        })
        .WithName("DeletePatient")
        .Produces<DeletePatientResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Delete Patient")
        .WithDescription("Delete Patient");
    }
}