using Emite.Common.Utility.Extensions;
using Emite.Common.Utility.Models;
using Emite.CCM.Infrastructure.Data;
using Emite.CCM.Core.Oidc;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Emite.Common.Core.Queries;

namespace Emite.CCM.Web.Areas.Admin.Queries.Scopes;

public record GetScopesQuery : BaseQuery, IRequest<PagedListResponse<OidcScope>>
{
}

public class GetScopesQueryHandler : IRequestHandler<GetScopesQuery, PagedListResponse<OidcScope>>
{
    private readonly IdentityContext _context;

    public GetScopesQueryHandler(IdentityContext context)
    {
        _context = context;
    }

    public Task<PagedListResponse<OidcScope>> Handle(GetScopesQuery request, CancellationToken cancellationToken) =>
        Task.FromResult(_context.Set<OidcScope>()
                      .AsNoTracking()
                      .ToPagedResponse(request.SearchColumns, request.SearchValue, request.SortColumn,
                                       request.SortOrder, request.PageNumber, request.PageSize));
}
