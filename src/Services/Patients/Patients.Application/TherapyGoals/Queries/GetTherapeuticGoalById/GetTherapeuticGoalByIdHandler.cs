namespace Patients.Application.TherapyGoals.Queries.GetTherapeuticGoalById;

public class GetTherapeuticGoalByIdHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService)
    : IQueryHandler<GetTherapeuticGoalByIdQuery, GetTherapeuticGoalByIdResult>
{
    public async Task<GetTherapeuticGoalByIdResult> Handle(GetTherapeuticGoalByIdQuery query, CancellationToken cancellationToken)
    {
        var currentUserId = currentUserService.UserId
            ?? throw new UnauthorizedAccessException("User is not authenticated.");

        var goal = await dbContext.TherapeuticGoals
            .AsNoTracking()
            .FirstOrDefaultAsync(g => g.Id == query.GoalId && g.TherapistId == currentUserId, cancellationToken);

        if (goal == null)
        {
            throw new TherapeuticGoalNotFoundException(query.GoalId);
        }

        return new GetTherapeuticGoalByIdResult(goal.ToTherapeuticGoalDto());
    }
}
