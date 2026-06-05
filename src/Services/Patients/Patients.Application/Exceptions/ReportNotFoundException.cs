using BuildingBlocks.Exceptions;

namespace Patients.Application.Exceptions;

public class ReportNotFoundException : NotFoundException
{
    public ReportNotFoundException(Guid id) : base("Report", id)
    {
    }
}
