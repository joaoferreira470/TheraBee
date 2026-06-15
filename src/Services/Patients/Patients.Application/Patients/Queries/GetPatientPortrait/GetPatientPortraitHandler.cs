namespace Patients.Application.Patients.Queries.GetPatientPortrait;

public class GetPatientPortraitHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUserService,
    IPortraitStorage portraitStorage)
    : IQueryHandler<GetPatientPortraitQuery, GetPatientPortraitResult>
{
    public async Task<GetPatientPortraitResult> Handle(
        GetPatientPortraitQuery query,
        CancellationToken cancellationToken)
    {
        var currentUserId = currentUserService.UserId
            ?? throw new UnauthorizedAccessException("User is not authenticated.");

        var patient = await dbContext.Patients.AsNoTracking().FirstOrDefaultAsync(
            item => item.Id == PatientId.Of(query.PatientId) && item.TherapistId == currentUserId,
            cancellationToken)
            ?? throw new PatientNotFoundException(query.PatientId);

        if (patient.PortraitStorageKey is null)
        {
            throw new PortraitNotFoundException("Patient");
        }

        var portrait = await portraitStorage.GetAsync(patient.PortraitStorageKey, cancellationToken)
            ?? throw new PortraitNotFoundException("Patient");

        return new GetPatientPortraitResult(portrait);
    }
}
