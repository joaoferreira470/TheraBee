namespace Patients.Domain.Events;

public record PatientUpdatedEvent(Patient patient) : IDomainEvent;
