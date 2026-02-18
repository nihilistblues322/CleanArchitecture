using Bookify.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Infrastructure.Authorization;

internal sealed class AuthorizationService(ApplicationDbContext context)
{
    public async Task<UserRolesResponse> GetRolesForUserAsync(string identityId)
    {
        var roles = await context.Set<User>()
            .Where(user => user.IdentityId == identityId)
            .Select(user => new UserRolesResponse
            {
                Id = user.Id,
                Roles = user.Roles.ToList()
            })
            .FirstAsync();


        return roles;
    }


    public async Task<HashSet<string>> GetPermissionsForUserAsync(string identityId)
    {
        var permissions = await context.Set<User>()
            .Where(u => u.IdentityId == identityId)
            .SelectMany(u => u.Roles.Select(r => r.Permissions))
            .FirstAsync();

        return permissions.Select(p => p.Name).ToHashSet();
    }
}