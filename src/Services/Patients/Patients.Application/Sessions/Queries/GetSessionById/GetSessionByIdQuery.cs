namespace Patients.Application.Sessions.Queries.GetSessionById;

public record GetSessionByIdQuery(Guid SessionId)
    : IQuery<GetSessionByIdResult>;

public record GetSessionByIdResult(SessionDto Session);
