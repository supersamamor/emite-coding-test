using Emite.Common.Core.Queries;
using Emite.Common.Utility.Models;
using Emite.CCM.Core.CCM;
using Emite.CCM.Infrastructure.Data;
using MediatR;
using Emite.Common.Utility.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Emite.CCM.Application.Features.CCM.Report.Queries;

public record GetReportQuery : BaseQuery, IRequest<PagedListResponse<ReportState>>;

public class GetReportQueryHandler : BaseQueryHandler<ApplicationContext, ReportState, GetReportQuery>, IRequestHandler<GetReportQuery, PagedListResponse<ReportState>>
{
    public GetReportQueryHandler(ApplicationContext context) : base(context)
    {
    }
		
}
