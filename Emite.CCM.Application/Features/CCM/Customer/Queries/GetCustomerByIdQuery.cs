using Emite.Common.Core.Queries;
using Emite.CCM.Core.CCM;
using Emite.CCM.Infrastructure.Data;
using LanguageExt;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Emite.CCM.Application.Features.CCM.Customer.Queries;

public record GetCustomerByIdQuery(string Id) : BaseQueryById(Id), IRequest<Option<CustomerState>>;

public class GetCustomerByIdQueryHandler(ApplicationContext context) : BaseQueryByIdHandler<ApplicationContext, CustomerState, GetCustomerByIdQuery>(context), IRequestHandler<GetCustomerByIdQuery, Option<CustomerState>>
{
		
}
