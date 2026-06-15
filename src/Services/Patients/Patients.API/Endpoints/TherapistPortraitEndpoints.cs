using Patients.Application.Therapists.Commands.DeleteCurrentTherapistPortrait;
using Patients.Application.Therapists.Commands.UploadCurrentTherapistPortrait;
using Patients.Application.Therapists.Queries.GetCurrentTherapistPortrait;

namespace Patients.API.Endpoints;

public record TherapistPortraitResponse(PortraitInfoDto Portrait);

public class TherapistPortraitEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/therapists/me/portrait", async (
            IFormFile file,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            await using var stream = new MemoryStream();
            await file.CopyToAsync(stream, cancellationToken);

            var result = await sender.Send(
                new UploadCurrentTherapistPortraitCommand(stream.ToArray(), file.ContentType),
                cancellationToken);

            return Results.Ok(new TherapistPortraitResponse(result.Portrait));
        })
        .DisableAntiforgery()
        .RequireAuthorization()
        .WithName("UploadCurrentTherapistPortrait")
        .Accepts<IFormFile>("multipart/form-data")
        .Produces<TherapistPortraitResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .WithSummary("Upload Current Therapist Portrait");

        app.MapGet("/therapists/me/portrait", async (
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(new GetCurrentTherapistPortraitQuery(), cancellationToken);
            return Results.File(result.Portrait.Content, result.Portrait.ContentType);
        })
        .RequireAuthorization()
        .WithName("GetCurrentTherapistPortrait")
        .Produces(StatusCodes.Status200OK, contentType: "image/webp")
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Get Current Therapist Portrait");

        app.MapDelete("/therapists/me/portrait", async (
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(new DeleteCurrentTherapistPortraitCommand(), cancellationToken);
            return Results.Ok(new TherapistPortraitResponse(result.Portrait));
        })
        .RequireAuthorization()
        .WithName("DeleteCurrentTherapistPortrait")
        .Produces<TherapistPortraitResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .WithSummary("Delete Current Therapist Portrait");
    }
}
