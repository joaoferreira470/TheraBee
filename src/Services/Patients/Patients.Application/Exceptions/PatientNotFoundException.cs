using BuildingBlocks.Exceptions;

namespace Patients.Application.Exceptions;

public class PatientNotFoundException : NotFoundException
{
    public PatientNotFoundException(Guid id) : base("Patient", id)
    {
    }

    public PatientNotFoundException(string name) : base("Patient", name)
    {
    }

}
