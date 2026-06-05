using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Patients.Infrastructure.Data.Configurations;

public class SessionGoalConfiguration : IEntityTypeConfiguration<SessionGoal>
{
    public void Configure(EntityTypeBuilder<SessionGoal> builder)
    {
        builder.HasKey(sessionGoal => sessionGoal.Id);

        builder.Property(sessionGoal => sessionGoal.SessionId)
            .IsRequired();

        builder.Property(sessionGoal => sessionGoal.TherapeuticGoalId)
            .IsRequired();

        builder.Property(sessionGoal => sessionGoal.TherapistId)
            .IsRequired();

        builder.HasIndex(sessionGoal => new { sessionGoal.SessionId, sessionGoal.TherapeuticGoalId })
            .IsUnique();
    }
}
