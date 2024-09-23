using Emite.Common.Core.Queries;
using Emite.CCM.Core.CCM;
using Emite.CCM.Infrastructure.Data;
using LanguageExt;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Emite.CCM.Application.Features.CCM.Call.Queries;

public record GetCallByIdQuery(string Id) : BaseQueryById(Id), IRequest<Option<CallState>>;

public class GetCallByIdQueryHandler(ApplicationContext context) : BaseQueryByIdHandler<ApplicationContext, CallState, GetCallByIdQuery>(context), IRequestHandler<GetCallByIdQuery, Option<CallState>>
{

    public override async Task<Option<CallState>> Handle(GetCallByIdQuery request, CancellationToken cancellationToken = default)
    {
        var call = await Context.Call
           .Where(e => e.Id == request.Id)
           .IgnoreQueryFilters()
           .AsNoTracking()
           .FirstOrDefaultAsync(cancellationToken);

        call!.Agent = await Context.Agent
           .Where(e => e.Id == call.AgentId).AsNoTracking()
           .FirstOrDefaultAsync(cancellationToken);

        call!.Customer = await Context.Customer
         .Where(e => e.Id == call.CustomerId).AsNoTracking()
         .FirstOrDefaultAsync(cancellationToken);

        return call;
    }

}
