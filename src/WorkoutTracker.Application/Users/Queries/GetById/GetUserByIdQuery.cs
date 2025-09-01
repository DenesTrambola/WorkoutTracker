namespace WorkoutTracker.Application.Users.Queries.GetById;

using WorkoutTracker.Application.Shared.Primitives.Messaging;

public sealed record GetUserByIdQuery(Guid Id)
    : IQuery<UserResponse>;
