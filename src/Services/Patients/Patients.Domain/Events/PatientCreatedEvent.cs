namespace Patients.Domain.Events;

public record PatientCreatedEvent(Patient patient) : IDomainEvent;
