using Patients.Application.Data;
using System.Reflection;

namespace Patients.Infrastructure.Data;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : base(options)
    {
    }

    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<TherapeuticGoal> TherapeuticGoals => Set<TherapeuticGoal>();
    public DbSet<Session> Sessions => Set<Session>();
    public DbSet<SessionGoal> SessionGoals => Set<SessionGoal>();
    public DbSet<User> Users => Set<User>();
    public DbSet<TherapistProfile> TherapistProfiles => Set<TherapistProfile>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }
}
