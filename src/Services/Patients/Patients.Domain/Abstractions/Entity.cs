
namespace Patients.Domain.Abstractions;

public abstract class Entity<T> : IEntity<T>
{
    public required T Id { get; set; }//coloquei como required
    public DateTime? CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? LastModified { get; set; }
    public string? LastModifiedBy { get; set; }
}
