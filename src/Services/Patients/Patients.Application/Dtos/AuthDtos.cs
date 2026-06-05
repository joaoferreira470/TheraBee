namespace Patients.Application.Dtos;

public record AuthUserDto(Guid Id, string Name, string Email, string Role);

public record AuthDto(string AccessToken, AuthUserDto User);

