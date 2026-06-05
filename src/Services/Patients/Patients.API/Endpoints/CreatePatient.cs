using Microsoft.AspNetCore.Mvc;
using Patients.Application.Patients.Commands.CreatePatient;

namespace Patients.API.Endpoints;

public record CreatePatientRequest(CreatePatientDto Patient);

public record CreatePatientResponse(Guid Id);

public class CreatePatient : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/patients", async ([FromBody] CreatePatientRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreatePatientCommand>();
            var result = await sender.Send(command);
            var response = result.Adapt<CreatePatientResponse>();

            return Results.Created($"/patients/{response.Id}", response);
        })
        .RequireAuthorization()
        .WithName("CreatePatient")
        .Produces<CreatePatientResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status409Conflict)
        .WithSummary("Create Patient")
        .WithDescription("Create Patient");
    }
}
