using Emite.Common.Core.Queries;
using Emite.Common.Utility.Models;
using Emite.CCM.Core.CCM;
using Emite.CCM.Infrastructure.Data;
using MediatR;
using Emite.Common.Utility.Extensions;
using Microsoft.EntityFrameworkCore;
using Emite.CCM.Application.DTOs;
using LanguageExt;

namespace Emite.CCM.Application.Features.CCM.Customer.Queries;

public record GetCustomerQuery : BaseQuery, IRequest<PagedListResponse<CustomerListDto>>;

public class GetCustomerQueryHandler(ApplicationContext context) : BaseQueryHandler<ApplicationContext, CustomerListDto, GetCustomerQuery>(context), IRequestHandler<GetCustomerQuery, PagedListResponse<CustomerListDto>>
{
	public override Task<PagedListResponse<CustomerListDto>> Handle(GetCustomerQuery request, CancellationToken cancellationToken = default)
	{
		return Task.FromResult(Context.Set<CustomerState>()
			.AsNoTracking().Select(e => new CustomerListDto()
			{
				Id = e.Id,
				LastModifiedDate = e.LastModifiedDate,
				Email = e.Email,
				LastContactDate = e.LastContactDate,
				Name = e.Name,
				PhoneNumber = e.PhoneNumber,
			})
			.ToPagedResponse(request.SearchColumns, request.SearchValue,
				request.SortColumn, request.SortOrder,
				request.PageNumber, request.PageSize));
	}	
}
