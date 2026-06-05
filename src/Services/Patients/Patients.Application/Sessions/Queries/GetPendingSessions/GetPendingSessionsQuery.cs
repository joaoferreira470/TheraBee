namespace Patients.Application.Sessions.Queries.GetPendingSessions;

public record GetPendingSessionsQuery
    : IQuery<GetPendingSessionsResult>;

public record GetPendingSessionsResult(IEnumerable<SessionDto> Sessions);
