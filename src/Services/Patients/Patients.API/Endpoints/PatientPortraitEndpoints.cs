using Microsoft.AspNetCore.Mvc;
using Patients.Application.Patients.Commands.DeletePatientPortrait;
using Patients.Application.Patients.Commands.UploadPatientPortrait;
using Patients.Application.Patients.Queries.GetPatientPortrait;

namespace Patients.API.Endpoints;

public record PatientPortraitResponse(PortraitInfoDto Portrait);

public class PatientPortraitEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/patients/{patientId:guid}/portrait", async (
            Guid patientId,
            [FromForm] IFormFile file,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            await using var stream = new MemoryStream();
            await file.CopyToAsync(stream, cancellationToken);

            var result = await sender.Send(
                new UploadPatientPortraitCommand(patientId, stream.ToArray(), file.ContentType),
                cancellationToken);

            return Results.Ok(new PatientPortraitResponse(result.Portrait));
        })
        .DisableAntiforgery()
        .RequireAuthorization()
        .WithName("UploadPatientPortrait")
        .Accepts<IFormFile>("multipart/form-data")
        .Produces<PatientPortraitResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Upload Patient Portrait");

        app.MapGet("/patients/{patientId:guid}/portrait", async (
            Guid patientId,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(new GetPatientPortraitQuery(patientId), cancellationToken);
            return Results.File(result.Portrait.Content, result.Portrait.ContentType);
        })
        .RequireAuthorization()
        .WithName("GetPatientPortrait")
        .Produces(StatusCodes.Status200OK, contentType: "image/webp")
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Get Patient Portrait");

        app.MapDelete("/patients/{patientId:guid}/portrait", async (
            Guid patientId,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(new DeletePatientPortraitCommand(patientId), cancellationToken);
            return Results.Ok(new PatientPortraitResponse(result.Portrait));
        })
        .RequireAuthorization()
        .WithName("DeletePatientPortrait")
        .Produces<PatientPortraitResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Delete Patient Portrait");
    }
}
