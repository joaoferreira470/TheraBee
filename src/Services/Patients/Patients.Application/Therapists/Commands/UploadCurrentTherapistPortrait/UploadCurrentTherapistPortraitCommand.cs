using FluentValidation;

namespace Patients.Application.Therapists.Commands.UploadCurrentTherapistPortrait;

public record UploadCurrentTherapistPortraitCommand(byte[] Content, string ContentType)
    : ICommand<UploadCurrentTherapistPortraitResult>;

public record UploadCurrentTherapistPortraitResult(PortraitInfoDto Portrait);

public class UploadCurrentTherapistPortraitCommandValidator : AbstractValidator<UploadCurrentTherapistPortraitCommand>
{
    public UploadCurrentTherapistPortraitCommandValidator()
    {
        RuleFor(command => command.Content).NotEmpty().Must(content => content.Length <= 5 * 1024 * 1024)
            .WithMessage("Portrait images cannot exceed 5 MB.");
        RuleFor(command => command.ContentType).NotEmpty();
    }
}
