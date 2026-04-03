using Bookify.Application.Abstractions.Caching;
using Bookify.Domain.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookify.Application.Abstractions.Behaviors;

internal sealed class QueryCachingBehavior<TRequest, TResponse>(ICacheService cache, ILogger<QueryCachingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICachedQuery
    where TResponse : Result
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        TResponse? cachedResult = await cache.GetAsync<TResponse>(request.CachedKey, cancellationToken);

        string name = typeof(TRequest).Name;
        if (cachedResult is not null)
        {
            logger.LogInformation("Cache hit for {Query}", name);
            return cachedResult;
        }

        logger.LogInformation("Cache miss for {Query}", name);

        var result = await next(cancellationToken);

        if (result.IsSuccess)
        {
            await cache.SetAsync(request.CachedKey, result, request.Expiration, cancellationToken);
        }

        return result;
    }
}