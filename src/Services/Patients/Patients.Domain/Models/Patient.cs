using Patients.Domain.Abstractions;
using Patients.Domain.ValueObjects;

namespace Patients.Domain.Models;

public class Patient : Aggregate<Guid>
{
    public string Name { get; set; } = default!;
    DateTime DateOfBirth { get; set; } = default!;
    public Address PatientAddress { get; set; } = default!;

    public string Diagnosis {  get; set; } = default!;
    public string Info { get; set;} = default!;
    public Guid TherapistId { get; set; } = default!;

}
