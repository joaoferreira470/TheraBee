using Patients.Application.TherapyGoals.Queries.GetTherapeuticGoalsByPatient;

namespace Patients.API.Endpoints;

public record GetTherapeuticGoalsByPatientResponse(IEnumerable<TherapeuticGoalDto> TherapeuticGoals);

public class GetTherapeuticGoalsByPatient : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/patients/{patientId}/therapy-goals", async (Guid patientId, ISender sender) =>
        {
            var result = await sender.Send(new GetTherapeuticGoalsByPatientQuery(patientId));
            var response = result.Adapt<GetTherapeuticGoalsByPatientResponse>();

            return Results.Ok(response);
        })
        .RequireAuthorization()
        .WithName("GetTherapeuticGoalsByPatient")
        .Produces<GetTherapeuticGoalsByPatientResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Get Therapeutic Goals By Patient")
        .WithDescription("Gets the authenticated therapist's therapeutic goals for a patient.");
    }
}
