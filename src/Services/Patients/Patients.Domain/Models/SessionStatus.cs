namespace Patients.Domain.Models;

public enum SessionStatus
{
    Scheduled = 0,
    Completed = 1,
    Cancelled = 2,
    PatientNoShow = 3,
    TherapistNoShow = 4,
    Rescheduled = 5
}
