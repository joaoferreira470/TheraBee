using Patients.Application.Patients.Queries.GetPatientsByTherapist;

namespace Patients.API.Endpoints;

public record GetPatientsByTherapistResponse(IEnumerable<PatientDto> Patients);

public class GetPatientsByTherapist : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/patients/me", async (ISender sender) =>
        {
            var result = await sender.Send(new GetPatientsByTherapistQuery());
            var response = result.Adapt<GetPatientsByTherapistResponse>();

            return Results.Ok(response);
        })
        .RequireAuthorization()
        .WithName("GetPatientsByTherapist")
        .Produces<GetPatientsByTherapistResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .WithSummary("Get My Patients")
        .WithDescription("Gets the authenticated therapist's patients ordered by name.");
    }
}
