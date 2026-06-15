using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Patients.Infrastructure.Data.Configurations;

public class TherapistProfileConfiguration : IEntityTypeConfiguration<TherapistProfile>
{
    public void Configure(EntityTypeBuilder<TherapistProfile> builder)
    {
        builder.HasKey(profile => profile.Id);

        builder.Property(profile => profile.UserId)
            .IsRequired();

        builder.HasIndex(profile => profile.UserId)
            .IsUnique();

        builder.HasOne<User>()
            .WithOne()
            .HasForeignKey<TherapistProfile>(profile => profile.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(profile => profile.ProfessionalName)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(profile => profile.Profession)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(profile => profile.Specialties)
            .HasMaxLength(500);

        builder.Property(profile => profile.ProfessionalNumber)
            .HasMaxLength(80);

        builder.Property(profile => profile.PhoneNumber)
            .HasMaxLength(40);

        builder.Property(profile => profile.Workplace)
            .HasMaxLength(180);

        builder.Property(profile => profile.ReportSignature)
            .HasMaxLength(500);

        builder.Property(profile => profile.PortraitStorageKey)
            .HasMaxLength(500);

        builder.Property(profile => profile.PortraitContentType)
            .HasMaxLength(100);
    }
}
