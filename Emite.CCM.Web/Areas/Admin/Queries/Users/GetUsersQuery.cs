using Emite.Common.Utility.Extensions;
using Emite.Common.Utility.Models;
using Emite.CCM.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Emite.Common.Core.Queries;
using Emite.CCM.Core.Identity;

namespace Emite.CCM.Web.Areas.Admin.Queries.Users;

public record GetUsersQuery : BaseQuery, IRequest<PagedListResponse<ApplicationUser>>
{
}

public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, PagedListResponse<ApplicationUser>>
{
    private readonly IdentityContext _context;

    public GetUsersQueryHandler(IdentityContext context)
    {
        _context = context;
    }

    public Task<PagedListResponse<ApplicationUser>> Handle(GetUsersQuery request, CancellationToken cancellationToken) =>
        Task.FromResult(_context.Users.AsNoTracking().ToPagedResponse(request.SearchColumns, request.SearchValue,
                                                            request.SortColumn, request.SortOrder,
                                                            request.PageNumber, request.PageSize));
}
