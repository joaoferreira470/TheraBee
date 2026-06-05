namespace Patients.Application.Exceptions;

public class DuplicatePatientException : Exception
{
    public DuplicatePatientException()
        : base("A patient with matching details already exists for this therapist.")
    {
    }
}
