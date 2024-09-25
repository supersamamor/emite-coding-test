using MediatR;
using Emite.CCM.Application.Services;
using Emite.CCM.Core.CCM;
using System.ComponentModel.DataAnnotations;

namespace Emite.CCM.Application.Features.CCM.CallsPerAgent.Queries;

public record GetTicketsViaElasticSearchQuery : IRequest<IReadOnlyCollection<TicketState>?> {
    [Required]
    public string? Status { get; set; } 
    [Required]
    public int PageSize { get; set; } 
    [Required]
    public int PageNumber { get; set; }
}

public class GetTicketsViaElasticSearchQueryHandler(ElasticSearchService elasticSearchService) : IRequestHandler<GetTicketsViaElasticSearchQuery, IReadOnlyCollection<TicketState>?>
{
    public async Task<IReadOnlyCollection<TicketState>?> Handle(GetTicketsViaElasticSearchQuery request, CancellationToken cancellationToken = default)
    {
        return (await elasticSearchService.SearchTicketsWithPaginationAsync(request.Status!, request.PageNumber, request.PageSize))?.Documents;
    }
}
