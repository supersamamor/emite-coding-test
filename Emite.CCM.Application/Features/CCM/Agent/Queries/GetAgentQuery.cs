using Emite.Common.Core.Queries;
using Emite.Common.Utility.Models;
using Emite.CCM.Core.CCM;
using Emite.CCM.Infrastructure.Data;
using MediatR;
using Emite.Common.Utility.Extensions;
using Microsoft.EntityFrameworkCore;
using Emite.CCM.Application.DTOs;
using LanguageExt;
using Emite.CCM.Application.Services;

namespace Emite.CCM.Application.Features.CCM.Agent.Queries;

public record GetAgentQuery : BaseQuery, IRequest<PagedListResponse<AgentListDto>>;

public class GetAgentQueryHandler(ApplicationContext context, CacheService? cacheService) : BaseQueryHandler<ApplicationContext, AgentListDto, GetAgentQuery>(context), IRequestHandler<GetAgentQuery, PagedListResponse<AgentListDto>>
{
    public override Task<PagedListResponse<AgentListDto>> Handle(GetAgentQuery request, CancellationToken cancellationToken = default)
    {
        var cachedData = cacheService?.GetCacheViaQueryAndParameters<PagedListResponse<AgentListDto>>(nameof(GetAgentQuery), request.CacheQueryParameter);
        if (cachedData != null)
        {
            return Task.FromResult(cachedData);
        }
        var response = Context.Set<AgentState>()
            .AsNoTracking()
            .Select(e => new AgentListDto()
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

        // Set cache if cacheService is available
        cacheService?.SetCache(nameof(GetAgentQuery), request.CacheQueryParameter, response);
        return Task.FromResult(response);

    }
}
