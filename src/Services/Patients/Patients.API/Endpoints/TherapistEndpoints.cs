using Patients.Application.Therapists.Commands.UpdateCurrentTherapist;
using Patients.Application.Therapists.Queries.GetCurrentTherapist;

namespace Patients.API.Endpoints;

public record GetCurrentTherapistResponse(TherapistProfileDto Therapist);

public record UpdateCurrentTherapistRequest(
    string ProfessionalName,
    string Profession,
    string? Specialties,
    string? ProfessionalNumber,
    string? PhoneNumber,
    string? Workplace,
    string? ReportSignature);

public record UpdateCurrentTherapistResponse(TherapistProfileDto Therapist);

public class TherapistEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/therapists/me", async (ISender sender) =>
        {
            var result = await sender.Send(new GetCurrentTherapistQuery());

            return Results.Ok(new GetCurrentTherapistResponse(result.Therapist));
        })
        .RequireAuthorization()
        .WithName("GetCurrentTherapist")
        .Produces<GetCurrentTherapistResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .WithSummary("Get Current Therapist")
        .WithDescription("Returns the authenticated therapist profile.");

        app.MapPut("/therapists/me", async (UpdateCurrentTherapistRequest request, ISender sender) =>
        {
            var command = request.Adapt<UpdateCurrentTherapistCommand>();
            var result = await sender.Send(command);

            return Results.Ok(new UpdateCurrentTherapistResponse(result.Therapist));
        })
        .RequireAuthorization()
        .WithName("UpdateCurrentTherapist")
        .Produces<UpdateCurrentTherapistResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .WithSummary("Update Current Therapist")
        .WithDescription("Updates the authenticated therapist profile.");
    }
}

