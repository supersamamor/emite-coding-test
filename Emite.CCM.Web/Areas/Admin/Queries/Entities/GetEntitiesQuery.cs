using Emite.Common.Utility.Extensions;
using Emite.Common.Utility.Models;
using Emite.CCM.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Emite.Common.Core.Queries;
using Emite.CCM.Core.Identity;

namespace Emite.CCM.Web.Areas.Admin.Queries.Entities;

public record GetEntitiesQuery : BaseQuery, IRequest<PagedListResponse<Entity>>
{
}

public class GetEntitiesQueryHandler : IRequestHandler<GetEntitiesQuery, PagedListResponse<Entity>>
{
    private readonly IdentityContext _context;

    public GetEntitiesQueryHandler(IdentityContext context)
    {
        _context = context;
    }

    public Task<PagedListResponse<Entity>> Handle(GetEntitiesQuery request, CancellationToken cancellationToken) =>
        Task.FromResult(_context.Entities.AsNoTracking().ToPagedResponse(request.SearchColumns, request.SearchValue,
                                                               request.SortColumn, request.SortOrder,
                                                               request.PageNumber, request.PageSize));
}
