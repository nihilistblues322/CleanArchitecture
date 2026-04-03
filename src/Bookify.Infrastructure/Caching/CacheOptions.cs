using Microsoft.Extensions.Caching.Distributed;

namespace Bookify.Infrastructure.Caching;

public class CacheOptions
{
    public static DistributedCacheEntryOptions DefaultExpiration => new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1)
    };

    public static DistributedCacheEntryOptions Create(TimeSpan? exp) =>
        exp is not null ? new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = exp } : DefaultExpiration;
}