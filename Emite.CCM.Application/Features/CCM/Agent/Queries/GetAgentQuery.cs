using Emite.Common.Core.Queries;
using Emite.Common.Utility.Models;
using Emite.CCM.Core.CCM;
using Emite.CCM.Infrastructure.Data;
using MediatR;
using Emite.Common.Utility.Extensions;
using Microsoft.EntityFrameworkCore;
using Emite.CCM.Application.DTOs;
using LanguageExt;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Emite.CCM.Application.Features.CCM.Agent.Queries;

public record GetAgentQuery : BaseQuery, IRequest<PagedListResponse<AgentListDto>>;

public class GetAgentQueryHandler(ApplicationContext context, IMemoryCache cache, IOptions<CacheSettings> cacheSettings) : BaseCachedQueryHandler<ApplicationContext, AgentListDto, GetAgentQuery, IMemoryCache>(context, cache, cacheSettings.Value), IRequestHandler<GetAgentQuery, PagedListResponse<AgentListDto>>
{
    public override Task<PagedListResponse<AgentListDto>> Handle(GetAgentQuery request, CancellationToken cancellationToken = default)
    {
        var cacheKey = request.GenerateCacheKey(nameof(GetAgentQuery));
        if (Cache.TryGetValue(cacheKey, out PagedListResponse<AgentListDto>? cachedResponse))
        {
            return Task.FromResult(cachedResponse!);
        }
        else
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(DefaultCacheDurationMinutes)
            };
            var response = Context.Set<AgentState>()
               .AsNoTracking().Select(e => new AgentListDto()
               {
                   Id = e.Id,
                   LastModifiedDate = e.LastModifiedDate,
                   Email = e.Email,
                   Name = e.Name,
                   PhoneExtension = e.PhoneExtension,
                   Status = e.Status,
               })
               .ToPagedResponse(request.SearchColumns, request.SearchValue,
                   request.SortColumn, request.SortOrder,
                   request.PageNumber, request.PageSize);
            Cache.Set(cacheKey, response, cacheEntryOptions);
            return Task.FromResult(response);
        }

    }
}
