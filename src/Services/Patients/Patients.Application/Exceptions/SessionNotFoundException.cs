using BuildingBlocks.Exceptions;

namespace Patients.Application.Exceptions;

public class SessionNotFoundException : NotFoundException
{
    public SessionNotFoundException(Guid id) : base("Session", id)
    {
    }
}
