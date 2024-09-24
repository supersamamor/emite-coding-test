using MediatR;
using Emite.CCM.Application.Services;
using Emite.CCM.Core.CCM;

namespace Emite.CCM.Application.Features.CCM.CallsPerAgent.Queries;

public record GetTicketsViaElasticSearchQuery : IRequest<IReadOnlyCollection<TicketState>?> {
    public string Status { get; set; } = "";
    public int PageSize { get; set; } = 0;
    public int PageNumber { get; set; }
}

public class GetTicketsViaElasticSearchQueryHandler(ElasticSearchService elasticSearchService) : IRequestHandler<GetTicketsViaElasticSearchQuery, IReadOnlyCollection<TicketState>?>
{
    public async Task<IReadOnlyCollection<TicketState>?> Handle(GetTicketsViaElasticSearchQuery request, CancellationToken cancellationToken = default)
    {
        return (await elasticSearchService.SearchTicketsWithPaginationAsync(request.Status, request.PageNumber, request.PageSize))?.Documents;
    }
}
