using Patients.Application.Patients.Commands.UpdatePatient;

namespace Patients.API.Endpoints;

public record UpdatePatientRequest(UpdatePatientDto Patient);

public record UpdatePatientResponse(bool IsSuccess);

public class UpdatePatient : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/patients", async (UpdatePatientRequest request, ISender sender) =>
        {
            var command = request.Adapt<UpdatePatientCommand>();
            var result = await sender.Send(command);
            var response = result.Adapt<UpdatePatientResponse>();

            return Results.Ok(response);
        })
        .RequireAuthorization()
        .WithName("UpdatePatient")
        .Produces<UpdatePatientResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Update Patient")
        .WithDescription("Update Patient");
    }
}
