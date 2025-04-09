using Patients.Application.Patients.Queries.GetPatientsByTherapist;

namespace Patients.API.Endpoints;

public record GetPatientsByTherapistResponse(IEnumerable<PatientDto> Patients);
public class GetPatientsByTherapist : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/patients/therapist/{id}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetPatientsByTherapistQuery(id));

            var response = result.Adapt<GetPatientsByTherapistResponse>();

            return Results.Ok(response);
        })
        .WithName("GetPatientsByTherapist")
        .Produces<GetPatientsByTherapistResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Get Patients By Therapist")
        .WithDescription("Get Patients By Therapist");
    }
}


