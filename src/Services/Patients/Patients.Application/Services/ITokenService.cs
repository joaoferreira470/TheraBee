namespace Patients.Application.Services;

public interface ITokenService
{
    string CreateToken(User user);
}

