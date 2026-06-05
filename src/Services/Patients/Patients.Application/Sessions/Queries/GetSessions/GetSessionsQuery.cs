namespace Patients.Application.Sessions.Queries.GetSessions;

public record GetSessionsQuery()
    : IQuery<GetSessionsResult>;

public record GetSessionsResult(IEnumerable<SessionDto> Sessions);
