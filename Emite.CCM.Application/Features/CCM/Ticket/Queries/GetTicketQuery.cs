using Emite.Common.Core.Queries;
using Emite.Common.Utility.Models;
using Emite.CCM.Core.CCM;
using Emite.CCM.Infrastructure.Data;
using MediatR;
using Emite.Common.Utility.Extensions;
using Microsoft.EntityFrameworkCore;
using Emite.CCM.Application.DTOs;
using LanguageExt;

namespace Emite.CCM.Application.Features.CCM.Ticket.Queries;

public record GetTicketQuery : BaseQuery, IRequest<PagedListResponse<TicketListDto>>;

public class GetTicketQueryHandler(ApplicationContext context) : BaseQueryHandler<ApplicationContext, TicketListDto, GetTicketQuery>(context), IRequestHandler<GetTicketQuery, PagedListResponse<TicketListDto>>
{
	public override async Task<PagedListResponse<TicketListDto>> Handle(GetTicketQuery request, CancellationToken cancellationToken = default)
	{
		var pagedList = Context.Set<TicketState>().Include(l=>l.Agent).Include(l=>l.Customer)
			.AsNoTracking().Select(e => new TicketListDto()
			{
				Id = e.Id,
				LastModifiedDate = e.LastModifiedDate,
				AgentId = e.Agent == null ? "" : e.Agent!.Id,
				CreatedAt = e.CreatedAt,
				CustomerId = e.Customer == null ? "" : e.Customer!.Id,
				Description = e.Description,
				Priority = e.Priority,
				Resolution = e.Resolution,
				Status = e.Status,
				UpdatedAt = e.UpdatedAt,
			})
			.ToPagedResponse(request.SearchColumns, request.SearchValue,
				request.SortColumn, request.SortOrder,
				request.PageNumber, request.PageSize);
		foreach (var item in pagedList.Data)
		{
			item.StatusBadge = await Helpers.ApprovalHelper.GetApprovalStatus(Context, item.Id, cancellationToken);
		}
		return pagedList;
	}	
}
