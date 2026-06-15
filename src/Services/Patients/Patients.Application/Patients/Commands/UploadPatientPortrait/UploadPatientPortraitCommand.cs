using FluentValidation;

namespace Patients.Application.Patients.Commands.UploadPatientPortrait;

public record UploadPatientPortraitCommand(Guid PatientId, byte[] Content, string ContentType)
    : ICommand<UploadPatientPortraitResult>;

public record UploadPatientPortraitResult(PortraitInfoDto Portrait);

public class UploadPatientPortraitCommandValidator : AbstractValidator<UploadPatientPortraitCommand>
{
    public UploadPatientPortraitCommandValidator()
    {
        RuleFor(command => command.PatientId).NotEmpty();
        RuleFor(command => command.Content).NotEmpty().Must(content => content.Length <= 5 * 1024 * 1024)
            .WithMessage("Portrait images cannot exceed 5 MB.");
        RuleFor(command => command.ContentType).NotEmpty();
    }
}
