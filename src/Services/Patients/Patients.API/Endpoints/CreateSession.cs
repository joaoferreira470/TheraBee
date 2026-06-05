using Microsoft.AspNetCore.Mvc;
using Patients.Application.Sessions.Commands.CreateSession;

namespace Patients.API.Endpoints;

public record CreateSessionRequest(CreateSessionDto Session);
public record CreateSessionResponse(Guid Id);

public class CreateSession : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/patients/{patientId}/sessions", async (Guid patientId, [FromBody] CreateSessionRequest request, ISender sender) =>
        {
            var result = await sender.Send(new CreateSessionCommand(patientId, request.Session));
            var response = result.Adapt<CreateSessionResponse>();

            return Results.Created($"/sessions/{response.Id}", response);
        })
        .RequireAuthorization()
        .WithName("CreateSession")
        .Produces<CreateSessionResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Create Session")
        .WithDescription("Creates a new therapy session for a patient owned by the authenticated therapist.");
    }
}
