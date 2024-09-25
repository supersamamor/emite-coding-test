using Emite.Common.Core.Queries;
using Emite.CCM.Core.CCM;
using Emite.CCM.Infrastructure.Data;
using LanguageExt;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Emite.CCM.Application.Services;
namespace Emite.CCM.Application.Features.CCM.Agent.Queries;

public record GetAgentByIdQuery(string Id) : BaseQueryById(Id), IRequest<Option<AgentState>>;

public class GetAgentByIdQueryHandler(ApplicationContext context, CacheService? cacheService) : BaseQueryByIdHandler<ApplicationContext, AgentState, GetAgentByIdQuery>(context), IRequestHandler<GetAgentByIdQuery, Option<AgentState>>
{
    public override async Task<Option<AgentState>> Handle(GetAgentByIdQuery request, CancellationToken cancellationToken = default)
    {
        var cachedData = cacheService?.GetCacheViaQueryAndParameters<AgentState>(nameof(GetAgentByIdQuery), request.Id);
        if (cachedData != null)
        {
            return cachedData;
        }
        var response = await Context.Agent.Where(e => e.Id == request.Id).AsNoTracking().SingleAsync(cancellationToken);
        cacheService?.SetCache(nameof(GetAgentByIdQuery), request.Id, response);
        return response;
    }          
}
