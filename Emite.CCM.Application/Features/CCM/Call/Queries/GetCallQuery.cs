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

public record GetCallQuery : BaseQuery, IRequest<PagedListResponse<CallListDto>>;

public class GetCallQueryHandler(ApplicationContext context) : BaseQueryHandler<ApplicationContext, CallListDto, GetCallQuery>(context), IRequestHandler<GetCallQuery, PagedListResponse<CallListDto>>
{
	public override Task<PagedListResponse<CallListDto>> Handle(GetCallQuery request, CancellationToken cancellationToken = default)
	{
		return Task.FromResult(Context.Set<CallState>().Include(l=>l.Agent).Include(l=>l.Customer)
			.AsNoTracking().Select(e => new CallListDto()
			{
				Id = e.Id,
				LastModifiedDate = e.LastModifiedDate,
				AgentId = e.Agent == null ? "" : e.Agent!.Id,
				CustomerId = e.Customer == null ? "" : e.Customer!.Id,
				EndTime = e.EndTime,
				Notes = e.Notes,
				StartTime = e.StartTime,
				Status = e.Status,
			})
			.ToPagedResponse(request.SearchColumns, request.SearchValue,
				request.SortColumn, request.SortOrder,
				request.PageNumber, request.PageSize));
	}	
}
