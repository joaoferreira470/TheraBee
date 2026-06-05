using Patients.Application.Services;

namespace Patients.API.Services;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public Guid? UserId
    {
        get
        {
            var value = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

            return Guid.TryParse(value, out var userId)
                ? userId
                : null;
        }
    }
}

