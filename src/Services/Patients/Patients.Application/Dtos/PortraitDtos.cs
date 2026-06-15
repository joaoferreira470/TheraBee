namespace Patients.Application.Dtos;

public record PortraitInfoDto(bool HasPortrait, DateTime? UpdatedAt);

public record PortraitContentDto(byte[] Content, string ContentType);
