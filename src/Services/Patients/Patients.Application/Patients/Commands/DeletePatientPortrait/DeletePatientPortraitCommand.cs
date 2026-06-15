namespace Patients.Application.Patients.Commands.DeletePatientPortrait;

public record DeletePatientPortraitCommand(Guid PatientId) : ICommand<DeletePatientPortraitResult>;

public record DeletePatientPortraitResult(PortraitInfoDto Portrait);
