namespace Patients.Application.Services;

public record StoredPortrait(string StorageKey, string ContentType);

public interface IPortraitStorage
{
    Task<StoredPortrait> SaveAsync(
        byte[] content,
        string contentType,
        string ownerType,
        Guid ownerId,
        CancellationToken cancellationToken);

    Task<PortraitContentDto?> GetAsync(string storageKey, CancellationToken cancellationToken);

    Task DeleteAsync(string storageKey, CancellationToken cancellationToken);
}
