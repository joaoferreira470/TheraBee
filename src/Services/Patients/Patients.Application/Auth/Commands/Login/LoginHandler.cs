namespace Patients.Application.Auth.Commands.Login;

public class LoginHandler(
    IApplicationDbContext dbContext,
    IPasswordHasher passwordHasher,
    ITokenService tokenService)
    : ICommandHandler<LoginCommand, LoginResult>
{
    public async Task<LoginResult> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var normalizedEmail = command.Email.Trim().ToLowerInvariant();
        var user = await dbContext.Users.FirstOrDefaultAsync(user => user.Email == normalizedEmail, cancellationToken);

        if (user is null || !passwordHasher.Verify(command.Password, user.PasswordHash))
        {
            throw new InvalidOperationException("Invalid email or password.");
        }

        var auth = new AuthDto(
            tokenService.CreateToken(user),
            new AuthUserDto(user.Id, user.Name, user.Email, user.Role));

        return new LoginResult(auth);
    }
}

