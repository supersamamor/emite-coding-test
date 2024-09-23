using Emite.Common.Core.Queries;
using Emite.Common.Utility.Models;
using Emite.CCM.Core.CCM;
using Emite.CCM.Infrastructure.Data;
using MediatR;
using Emite.Common.Utility.Extensions;
using Microsoft.EntityFrameworkCore;
using Emite.CCM.Application.DTOs;
using LanguageExt;

namespace Emite.CCM.Application.Features.CCM.Call.Queries;

public record GetCallQuery : BaseQuery, IRequest<PagedListResponse<CallListDto>>
{
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
    public string? Status { get; set; }
    public string? AgentId { get; set; }
}

public class GetCallQueryHandler(ApplicationContext context) : BaseQueryHandler<ApplicationContext, CallListDto, GetCallQuery>(context), IRequestHandler<GetCallQuery, PagedListResponse<CallListDto>>
{
    public override Task<PagedListResponse<CallListDto>> Handle(GetCallQuery request, CancellationToken cancellationToken = default)
    {
        var query = Context.Set<CallState>().Include(l => l.Agent).Include(l => l.Customer)
            .AsNoTracking();
        if (request.DateFrom != null && request.DateFrom != DateTime.MinValue)
        {
            query = query.Where(l => l.StartTime >= request.DateFrom);
        }
        if (request.DateTo != null && request.DateTo != DateTime.MinValue)
        {
            query = query.Where(l => l.EndTime <= request.DateTo);
        }
        if (!string.IsNullOrEmpty(request.Status))
        {
            query = query.Where(l => l.Status == request.Status);
        }
        if (!string.IsNullOrEmpty(request.AgentId))
        {
            query = query.Where(l => l.AgentId == request.AgentId);
        }
        return Task.FromResult(query.Select(e => new CallListDto()
        {
            Id = e.Id,
            LastModifiedDate = e.LastModifiedDate,
            AgentId = e.Agent == null ? "" : e.Agent!.Id,
            CustomerId = e.Customer == null ? "" : e.Customer!.Id,
            EndTime = e.EndTime,
            Notes = e.Notes,
            StartTime = e.StartTime,
            Status = e.Status,
        }).ToPagedResponse(request.SearchColumns, request.SearchValue,
                request.SortColumn, request.SortOrder,
                request.PageNumber, request.PageSize));
    }
}
