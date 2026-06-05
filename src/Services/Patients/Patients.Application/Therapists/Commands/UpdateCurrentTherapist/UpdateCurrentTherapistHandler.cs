namespace Patients.Application.Therapists.Commands.UpdateCurrentTherapist;

public class UpdateCurrentTherapistHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUserService)
    : ICommandHandler<UpdateCurrentTherapistCommand, UpdateCurrentTherapistResult>
{
    public async Task<UpdateCurrentTherapistResult> Handle(UpdateCurrentTherapistCommand command, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId
            ?? throw new InvalidOperationException("Authenticated user is required.");

        var therapist = await dbContext.TherapistProfiles
            .FirstOrDefaultAsync(profile => profile.UserId == userId, cancellationToken)
            ?? throw new InvalidOperationException("Therapist profile was not found.");

        therapist.ProfessionalName = command.ProfessionalName.Trim();
        therapist.Profession = command.Profession.Trim();
        therapist.Specialties = command.Specialties?.Trim();
        therapist.ProfessionalNumber = command.ProfessionalNumber?.Trim();
        therapist.PhoneNumber = command.PhoneNumber?.Trim();
        therapist.Workplace = command.Workplace?.Trim();
        therapist.ReportSignature = command.ReportSignature?.Trim();

        await dbContext.SaveChangesAsync(cancellationToken);

        return new UpdateCurrentTherapistResult(therapist.ToDto());
    }
}

