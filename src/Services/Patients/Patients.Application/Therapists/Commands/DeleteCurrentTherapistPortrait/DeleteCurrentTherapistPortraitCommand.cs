namespace Patients.Application.Therapists.Commands.DeleteCurrentTherapistPortrait;

public record DeleteCurrentTherapistPortraitCommand() : ICommand<DeleteCurrentTherapistPortraitResult>;

public record DeleteCurrentTherapistPortraitResult(PortraitInfoDto Portrait);
