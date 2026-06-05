using Patients.Application.Patients.Queries.GetDuplicatePatients;

namespace Patients.API.Endpoints;

public record GetDuplicatePatientsResponse(IEnumerable<PatientDto> Patients);

public class GetDuplicatePatients : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/patients/duplicates", async (string name, DateTime dateOfBirth, ISender sender) =>
        {
            var result = await sender.Send(new GetDuplicatePatientsQuery(name, dateOfBirth));
            var response = result.Adapt<GetDuplicatePatientsResponse>();

            return Results.Ok(response);
        })
        .RequireAuthorization()
        .WithName("GetDuplicatePatients")
        .Produces<GetDuplicatePatientsResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .WithSummary("Get Duplicate Patients")
        .WithDescription("Checks for possible duplicate patients for the authenticated therapist.");
    }
}
