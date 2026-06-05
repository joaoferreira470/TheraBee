namespace Patients.Application.Sessions.Queries.GetPendingRegistrationSessions;

public record GetPendingRegistrationSessionsQuery()
    : IQuery<GetPendingRegistrationSessionsResult>;

public record GetPendingRegistrationSessionsResult(IEnumerable<SessionDto> Sessions);
