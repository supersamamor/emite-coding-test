using Emite.Common.Utility.Extensions;
using Emite.Common.Utility.Models;
using Emite.CCM.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Emite.Common.Core.Queries;
using Emite.CCM.Core.CCM;

namespace Emite.CCM.Web.Areas.Admin.Queries.Entities;

public record GetBatchUploadJobsQuery : BaseQuery, IRequest<PagedListResponse<UploadProcessorState>>
{
}

public class GetBatchUploadJobsQueryHandler(ApplicationContext context) : IRequestHandler<GetBatchUploadJobsQuery, PagedListResponse<UploadProcessorState>>
{

    public Task<PagedListResponse<UploadProcessorState>> Handle(GetBatchUploadJobsQuery request, CancellationToken cancellationToken) =>
        Task.FromResult(context.UploadProcessor.AsNoTracking().ToPagedResponse(request.SearchColumns, request.SearchValue,
                                                               request.SortColumn, request.SortOrder,
                                                               request.PageNumber, request.PageSize));
}
