using Emite.Common.Core.Queries;
using Emite.CCM.Core.CCM;
using Emite.CCM.Infrastructure.Data;
using LanguageExt;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Emite.CCM.Application.Features.CCM.Agent.Queries;

public record GetAgentByIdQuery(string Id) : BaseQueryById(Id), IRequest<Option<AgentState>>;

public class GetAgentByIdQueryHandler(ApplicationContext context) : BaseQueryByIdHandler<ApplicationContext, AgentState, GetAgentByIdQuery>(context), IRequestHandler<GetAgentByIdQuery, Option<AgentState>>
{
		
}
