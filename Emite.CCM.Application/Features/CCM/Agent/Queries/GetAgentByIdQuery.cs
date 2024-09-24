using Emite.Common.Core.Queries;
using Emite.CCM.Core.CCM;
using Emite.CCM.Infrastructure.Data;
using LanguageExt;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
namespace Emite.CCM.Application.Features.CCM.Agent.Queries;

public record GetAgentByIdQuery(string Id) : BaseCachedQueryById(Id), IRequest<Option<AgentState>>;

public class GetAgentByIdQueryHandler(ApplicationContext context, IMemoryCache cache, IOptions<CacheSettings> cacheSettings) : BaseCachedQueryByIdHandler<ApplicationContext, AgentState, GetAgentByIdQuery, IMemoryCache>(context, cache, cacheSettings.Value), IRequestHandler<GetAgentByIdQuery, Option<AgentState>>
{
    public override async Task<Option<AgentState>> Handle(GetAgentByIdQuery request, CancellationToken cancellationToken = default)
    {
        var cacheKey = request.GenerateCacheKey(nameof(GetAgentByIdQuery));
        if (Cache.TryGetValue(cacheKey, out AgentState? cachedResponse))
        {
            return cachedResponse;
        }
        else
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(DefaultCacheDurationMinutes)
            };
            var response = await Context.Agent.Where(e => e.Id == request.Id).AsNoTracking().SingleAsync(cancellationToken);
            Cache.Set(cacheKey, response, cacheEntryOptions);
            return response;
        }
    }
          
}
