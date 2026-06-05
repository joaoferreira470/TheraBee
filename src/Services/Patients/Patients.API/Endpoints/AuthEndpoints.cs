using Patients.Application.Auth.Commands.Login;
using Patients.Application.Auth.Commands.RegisterTherapist;

namespace Patients.API.Endpoints;

public record RegisterTherapistRequest(string Name, string Email, string Password, string Profession);

public record LoginRequest(string Email, string Password);

public record AuthResponse(AuthDto Auth);

public class AuthEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/auth/register", async (RegisterTherapistRequest request, ISender sender) =>
        {
            var command = request.Adapt<RegisterTherapistCommand>();
            var result = await sender.Send(command);

            return Results.Created("/therapists/me", new AuthResponse(result.Auth));
        })
        .WithName("RegisterTherapist")
        .Produces<AuthResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Register Therapist")
        .WithDescription("Registers a therapist user and returns an access token.");

        app.MapPost("/auth/login", async (LoginRequest request, ISender sender) =>
        {
            var command = request.Adapt<LoginCommand>();
            var result = await sender.Send(command);

            return Results.Ok(new AuthResponse(result.Auth));
        })
        .WithName("Login")
        .Produces<AuthResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Login")
        .WithDescription("Authenticates a therapist and returns an access token.");

        app.MapPost("/auth/logout", () => Results.NoContent())
        .RequireAuthorization()
        .WithName("Logout")
        .Produces(StatusCodes.Status204NoContent)
        .WithSummary("Logout")
        .WithDescription("Ends the client session. JWT tokens should be discarded by the client.");
    }
}

