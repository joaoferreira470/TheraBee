namespace Patients.Application.Exceptions;

public class SessionSchedulingConflictException : Exception
{
    public SessionSchedulingConflictException(DateTime startDateTime, DateTime endDateTime)
        : base($"Another session already overlaps the requested slot from {startDateTime:u} to {endDateTime:u}.")
    {
    }
}
