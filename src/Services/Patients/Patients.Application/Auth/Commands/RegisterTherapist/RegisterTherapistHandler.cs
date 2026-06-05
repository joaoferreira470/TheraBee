namespace Patients.Application.Auth.Commands.RegisterTherapist;

public class RegisterTherapistHandler(
    IApplicationDbContext dbContext,
    IPasswordHasher passwordHasher,
    ITokenService tokenService)
    : ICommandHandler<RegisterTherapistCommand, RegisterTherapistResult>
{
    public async Task<RegisterTherapistResult> Handle(RegisterTherapistCommand command, CancellationToken cancellationToken)
    {
        var normalizedEmail = command.Email.Trim().ToLowerInvariant();
        var exists = await dbContext.Users.AnyAsync(user => user.Email == normalizedEmail, cancellationToken);

        if (exists)
        {
            throw new InvalidOperationException("A user with this email already exists.");
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = command.Name.Trim(),
            Email = normalizedEmail,
            PasswordHash = passwordHasher.Hash(command.Password),
            Role = "Therapist"
        };

        var therapistProfile = new TherapistProfile
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            ProfessionalName = user.Name,
            Profession = command.Profession?.Trim() ?? "Therapist"
        };

        dbContext.Users.Add(user);
        dbContext.TherapistProfiles.Add(therapistProfile);
        await dbContext.SaveChangesAsync(cancellationToken);

        var auth = new AuthDto(
            tokenService.CreateToken(user),
            new AuthUserDto(user.Id, user.Name, user.Email, user.Role));

        return new RegisterTherapistResult(auth);
    }
}
