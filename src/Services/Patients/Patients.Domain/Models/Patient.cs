using Patients.Domain.ValueObjects;

namespace Patients.Domain.Models;

public class Patient : Aggregate<PatientId>
{
    public string Name { get; set; } = default!;
    public DateTime DateOfBirth { get; set; } = default!;
    public Address PatientAddress { get; set; } = default!;

    public string Diagnosis {  get; set; } = default!;
    public string Info { get; set;} = default!;
    public Guid TherapistId { get; set; } = default!;

    public static Patient Create(PatientId id, string name, DateTime dateOfBirth, Address patientAddress, string diagnosis, string info, Guid therapistId)
    {
        var patient = new Patient
        {
            Id = id,
            Name = name,
            DateOfBirth = dateOfBirth,
            PatientAddress = patientAddress,
            Diagnosis = diagnosis,
            Info = info,
            TherapistId = therapistId
        };

        patient.AddDomainEvent(new PatientCreatedEvent(patient));

        return patient;
    }

    public void Update(string name, DateTime dateOfBirth, Address patientAddress, string diagnosis, string info, Guid therapistId)
    {
        Name = name;
        DateOfBirth = dateOfBirth;
        PatientAddress = patientAddress;
        Diagnosis = diagnosis;
        Info = info;
        TherapistId = therapistId;

        AddDomainEvent(new PatientUpdatedEvent(this));
    }
}
