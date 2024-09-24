using Emite.CCM.Infrastructure.Data;
using MediatR;
using Emite.Common.Utility.Extensions;
using Microsoft.EntityFrameworkCore;
using Emite.CCM.Application.DTOs;
using LanguageExt;

namespace Emite.CCM.Application.Features.CCM.CallsPerAgent.Queries;

public record GetCallsPerAgentQuery : IRequest<IList<CallsPerAgentListDto>>;

public class GetCallsPerAgentQueryHandler(ApplicationContext context) : IRequestHandler<GetCallsPerAgentQuery, IList<CallsPerAgentListDto>>
{
    public async Task<IList<CallsPerAgentListDto>> Handle(GetCallsPerAgentQuery request, CancellationToken cancellationToken = default)
    {
        return await context.Agent
                   .Select(agent => new CallsPerAgentListDto()
                   {
                       AgentName = agent.Name,
                       AgentEmail = agent.Email,
                       CallCount = agent.CallList == null ? 0 : agent.CallList.Count()
                   })
                   .ToListAsync(cancellationToken);
    }
}
