using Emite.Common.Core.Queries;
using Emite.CCM.Core.CCM;
using Emite.CCM.Infrastructure.Data;
using LanguageExt;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Emite.CCM.Application.Features.CCM.Ticket.Queries;

public record GetTicketByIdQuery(string Id) : BaseQueryById(Id), IRequest<Option<TicketState>>;

public class GetTicketByIdQueryHandler(ApplicationContext context) : BaseQueryByIdHandler<ApplicationContext, TicketState, GetTicketByIdQuery>(context), IRequestHandler<GetTicketByIdQuery, Option<TicketState>>
{
	
	public override async Task<Option<TicketState>> Handle(GetTicketByIdQuery request, CancellationToken cancellationToken = default)
	{
        var ticket = await Context.Ticket
          .Where(e => e.Id == request.Id)
          .IgnoreQueryFilters()
          .AsNoTracking()
          .FirstOrDefaultAsync(cancellationToken);
        ticket!.Agent = await Context.Agent
           .Where(e => e.Id == ticket.AgentId).AsNoTracking()
           .FirstOrDefaultAsync(cancellationToken);
        ticket!.Customer = await Context.Customer
         .Where(e => e.Id == ticket.CustomerId).AsNoTracking()
         .FirstOrDefaultAsync(cancellationToken);
        return ticket;
	}
}
