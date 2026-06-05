namespace Patients.Application.Reports.Commands.CreateReportDraft;

public class CreateReportDraftHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService, IReportDraftBuilder reportDraftBuilder)
    : ICommandHandler<CreateReportDraftCommand, CreateReportDraftResult>
{
    public async Task<CreateReportDraftResult> Handle(CreateReportDraftCommand command, CancellationToken cancellationToken)
    {
        var currentUserId = currentUserService.UserId
            ?? throw new UnauthorizedAccessException("User is not authenticated.");

        var report = await reportDraftBuilder.BuildAsync(command.PatientId, currentUserId, cancellationToken);

        dbContext.Reports.Add(report);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new CreateReportDraftResult(report.Id);
    }
}
