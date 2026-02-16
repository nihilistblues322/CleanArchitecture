using Bookify.Domain.Users;

namespace Bookify.Infrastructure.Repositories;

internal sealed class UserRepository(ApplicationDbContext context) : Repository<User>(context), IUserRepository
{
    public override void Add(User user)
    {
        foreach (var role in user.Roles)
        {
            context.Attach(role);
        }

        context.Add(user);
    }
}