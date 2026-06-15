using Microsoft.Extensions.Options;
using Patients.Application.Dtos;
using Patients.Application.Services;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;

namespace Patients.Infrastructure.Storage;

public class LocalPortraitStorage(IOptions<PortraitStorageOptions> options) : IPortraitStorage
{
    private const int PortraitSize = 512;
    private readonly string rootPath = Path.GetFullPath(options.Value.RootPath);

    public async Task<StoredPortrait> SaveAsync(
        byte[] content,
        string contentType,
        string ownerType,
        Guid ownerId,
        CancellationToken cancellationToken)
    {
        if (!IsSupportedContentType(contentType))
        {
            throw new InvalidOperationException("Only JPEG, PNG, and WebP portrait images are supported.");
        }

        await using var input = new MemoryStream(content);
        Image image;

        try
        {
            image = await Image.LoadAsync(input, cancellationToken);
        }
        catch (UnknownImageFormatException)
        {
            throw new InvalidOperationException("The uploaded file is not a valid image.");
        }

        using (image)
        {
            if (!IsSupportedFormat(image.Metadata.DecodedImageFormat?.Name))
            {
                throw new InvalidOperationException("Only JPEG, PNG, and WebP portrait images are supported.");
            }

            image.Mutate(context => context
                .AutoOrient()
                .Resize(new ResizeOptions
                {
                    Size = new Size(PortraitSize, PortraitSize),
                    Mode = ResizeMode.Crop,
                    Position = AnchorPositionMode.Center
                }));

            var storageKey = $"{ownerType}/{ownerId:N}/{Guid.NewGuid():N}.webp";
            var filePath = ResolvePath(storageKey);
            Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

            await image.SaveAsWebpAsync(
                filePath,
                new WebpEncoder { Quality = 82 },
                cancellationToken);

            return new StoredPortrait(storageKey, "image/webp");
        }
    }

    public async Task<PortraitContentDto?> GetAsync(
        string storageKey,
        CancellationToken cancellationToken)
    {
        var filePath = ResolvePath(storageKey);
        if (!File.Exists(filePath))
        {
            return null;
        }

        var content = await File.ReadAllBytesAsync(filePath, cancellationToken);
        return new PortraitContentDto(content, "image/webp");
    }

    public Task DeleteAsync(string storageKey, CancellationToken cancellationToken)
    {
        var filePath = ResolvePath(storageKey);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        return Task.CompletedTask;
    }

    private string ResolvePath(string storageKey)
    {
        var normalizedKey = storageKey.Replace('/', Path.DirectorySeparatorChar);
        var fullPath = Path.GetFullPath(Path.Combine(rootPath, normalizedKey));
        var rootPrefix = rootPath.EndsWith(Path.DirectorySeparatorChar)
            ? rootPath
            : rootPath + Path.DirectorySeparatorChar;

        if (!fullPath.StartsWith(rootPrefix, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("Invalid portrait storage key.");
        }

        return fullPath;
    }

    private static bool IsSupportedContentType(string contentType)
        => contentType.Equals("image/jpeg", StringComparison.OrdinalIgnoreCase)
            || contentType.Equals("image/png", StringComparison.OrdinalIgnoreCase)
            || contentType.Equals("image/webp", StringComparison.OrdinalIgnoreCase);

    private static bool IsSupportedFormat(string? formatName)
        => formatName is not null
            && (formatName.Equals(JpegFormat.Instance.Name, StringComparison.OrdinalIgnoreCase)
                || formatName.Equals(PngFormat.Instance.Name, StringComparison.OrdinalIgnoreCase)
                || formatName.Equals(WebpFormat.Instance.Name, StringComparison.OrdinalIgnoreCase));
}
