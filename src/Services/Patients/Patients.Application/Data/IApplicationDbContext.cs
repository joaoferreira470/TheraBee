using Microsoft.EntityFrameworkCore;
using Patients.Domain.Models;

namespace Patients.Application.Data;

public interface IApplicationDbContext
{
    DbSet<Patient> Patients { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
