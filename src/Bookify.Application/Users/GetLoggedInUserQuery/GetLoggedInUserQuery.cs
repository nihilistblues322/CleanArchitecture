using Bookify.Application.Abstractions.Messaging;

namespace Bookify.Application.Users.GetLoggedInUserQuery;

public sealed record GetLoggedInUserQuery : IQuery<UserResponse>;