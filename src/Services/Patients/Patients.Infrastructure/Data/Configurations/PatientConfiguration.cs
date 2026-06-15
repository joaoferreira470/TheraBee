using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Patients.Domain.Models;
using Patients.Domain.ValueObjects;

namespace Patients.Infrastructure.Data.Configurations;

public class PatientConfiguration : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id).HasConversion(
                                    patientID => patientID.Value,
                                    dbID => PatientId.Of(dbID));

        builder.Property(p => p.Name)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(p => p.DateOfBirth)
            .IsRequired();

        builder.Property(p => p.MainDiagnosis)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(p => p.Gender)
            .HasMaxLength(50);

        builder.Property(p => p.PhoneNumber)
            .HasMaxLength(40);

        builder.Property(p => p.Email)
            .HasMaxLength(254);

        builder.Property(p => p.CaregiverName)
            .HasMaxLength(150);

        builder.Property(p => p.CaregiverPhone)
            .HasMaxLength(40);

        builder.Property(p => p.ReferralReason)
            .HasMaxLength(500);

        builder.Property(p => p.GeneralNotes)
            .HasMaxLength(1000);

        builder.ComplexProperty(
            p => p.PatientAddress, addressBuilder =>
            {
                addressBuilder.Property(a => a.AddressLine)
                                        .HasMaxLength(180)
                                        .IsRequired();

                addressBuilder.Property(a => a.District)
                                        .HasMaxLength(25);

                addressBuilder.Property(a => a.Location)
                                        .HasMaxLength(25);

                addressBuilder.Property(a => a.ZipCode)
                                        .HasMaxLength(8)
                                        .IsRequired();
            });

        builder.Property(p => p.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(p => p.TherapistId)
            .IsRequired();

        builder.Property(p => p.PortraitStorageKey)
            .HasMaxLength(500);

        builder.Property(p => p.PortraitContentType)
            .HasMaxLength(100);
    }
}
