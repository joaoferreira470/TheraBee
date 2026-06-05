using Microsoft.EntityFrameworkCore;
using Patients.Domain.Models;

namespace Patients.Application.Data;

public interface IApplicationDbContext
{
    DbSet<Patient> Patients { get; }
    DbSet<TherapeuticGoal> TherapeuticGoals { get; }
    DbSet<User> Users { get; }
    DbSet<TherapistProfile> TherapistProfiles { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
