namespace Patients.Application.Patients.Queries.GetPatientsByTherapist;

public class GetPatientsByTherapistQueryHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetPatientsByTherapistQuery, GetPatientsByTherapistResult>
{
    public async Task<GetPatientsByTherapistResult> Handle(GetPatientsByTherapistQuery query, CancellationToken cancellationToken)
    {
        var patients = await dbContext.Patients
                                        .AsNoTracking()
                                        .Where(p => p.TherapistId == query.TherapistId)
                                        .OrderBy(p => p.Name)
                                        .ToListAsync(cancellationToken);

        return new GetPatientsByTherapistResult(patients.ToPatientDtoList());
    }
}
