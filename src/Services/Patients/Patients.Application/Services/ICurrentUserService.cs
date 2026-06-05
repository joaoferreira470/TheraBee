namespace Patients.Application.Services;

public interface ICurrentUserService
{
    Guid? UserId { get; }
}

