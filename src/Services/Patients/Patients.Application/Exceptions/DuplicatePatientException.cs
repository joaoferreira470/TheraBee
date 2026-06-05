namespace Patients.Application.Exceptions;

public class DuplicatePatientException : Exception
{
    public DuplicatePatientException(string name, DateTime dateOfBirth)
        : base($"A patient named \"{name}\" with date of birth \"{dateOfBirth:yyyy-MM-dd}\" already exists for this therapist.")
    {
    }
}
