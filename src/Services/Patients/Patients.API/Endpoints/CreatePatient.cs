using Patients.Application.Patients.Commands.CreatePatient;

namespace Patients.API.Endpoints;


//-Accepts a Request object.
//-Maps the request to a PatientCommand.
//-Uses MediatR to send the command to the corresponding handler.
//-Returns a reponse with the created Patient's ID.

public record CreatePatientRequest(PatientDto Patient);

public record CreatePatientResponse(Guid Id);
public class CreatePatient : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/patients", async (CreatePatientRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreatePatientCommand>();

            var result = await sender.Send(command);

            var response = result.Adapt<CreatePatientResponse>();

            return Results.Created($"/patients/{response.Id}", response);
        })
        .WithName("CreatePatient")
        .Produces<CreatePatientResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Create Patient")
        .WithDescription("Create Patient");
    }
}
