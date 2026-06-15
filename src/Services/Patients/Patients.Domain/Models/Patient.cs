using Patients.Domain.ValueObjects;

namespace Patients.Domain.Models;

public class Patient : Aggregate<PatientId>
{
    public string Name { get; set; } = default!;
    public DateTime DateOfBirth { get; set; } = default!;
    public Address PatientAddress { get; set; } = default!;
    public PatientStatus Status { get; set; } = PatientStatus.Active;

    public string MainDiagnosis { get; set; } = default!;
    public string? Gender { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? CaregiverName { get; set; }
    public string? CaregiverPhone { get; set; }
    public string? ReferralReason { get; set; }
    public string? GeneralNotes { get; set; }
    public Guid TherapistId { get; set; } = default!;
    public string? PortraitStorageKey { get; set; }
    public string? PortraitContentType { get; set; }
    public DateTime? PortraitUpdatedAt { get; set; }

    public static Patient Create(
        PatientId id,
        string name,
        DateTime dateOfBirth,
        Address patientAddress,
        string mainDiagnosis,
        string? generalNotes,
        Guid therapistId,
        string? gender = null,
        string? phoneNumber = null,
        string? email = null,
        string? caregiverName = null,
        string? caregiverPhone = null,
        string? referralReason = null)
    {
        var patient = new Patient
        {
            Id = id,
            Name = name,
            DateOfBirth = dateOfBirth,
            PatientAddress = patientAddress,
            Status = PatientStatus.Active,
            MainDiagnosis = mainDiagnosis,
            GeneralNotes = generalNotes,
            TherapistId = therapistId,
            Gender = gender,
            PhoneNumber = phoneNumber,
            Email = email,
            CaregiverName = caregiverName,
            CaregiverPhone = caregiverPhone,
            ReferralReason = referralReason
        };

        patient.AddDomainEvent(new PatientCreatedEvent(patient));

        return patient;
    }

    public void Update(
        string name,
        DateTime dateOfBirth,
        Address patientAddress,
        string mainDiagnosis,
        string? generalNotes,
        Guid therapistId,
        string? gender = null,
        string? phoneNumber = null,
        string? email = null,
        string? caregiverName = null,
        string? caregiverPhone = null,
        string? referralReason = null)
    {
        Name = name;
        DateOfBirth = dateOfBirth;
        PatientAddress = patientAddress;
        MainDiagnosis = mainDiagnosis;
        GeneralNotes = generalNotes;
        TherapistId = therapistId;
        Gender = gender;
        PhoneNumber = phoneNumber;
        Email = email;
        CaregiverName = caregiverName;
        CaregiverPhone = caregiverPhone;
        ReferralReason = referralReason;

        AddDomainEvent(new PatientUpdatedEvent(this));
    }

    public void UpdateStatus(PatientStatus status)
    {
        Status = status;
        AddDomainEvent(new PatientUpdatedEvent(this));
    }

    public void SetPortrait(string storageKey, string contentType, DateTime updatedAt)
    {
        PortraitStorageKey = storageKey;
        PortraitContentType = contentType;
        PortraitUpdatedAt = updatedAt;
        AddDomainEvent(new PatientUpdatedEvent(this));
    }

    public void RemovePortrait()
    {
        PortraitStorageKey = null;
        PortraitContentType = null;
        PortraitUpdatedAt = null;
        AddDomainEvent(new PatientUpdatedEvent(this));
    }
}
