using Emite.CCM.Application.Features.CCM.Ticket.Queries;
using Emite.CCM.Web.Areas.Admin.Models;
using Emite.CCM.Web.Areas.Admin.Queries.AuditTrail;
using Emite.CCM.Web.Areas.CCM.Models;
using Emite.CCM.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Emite.CCM.Web.Areas.CCM.Pages.Ticket;

[Authorize(Policy = Permission.Ticket.History)]
public class HistoryModel : BasePageModel<HistoryModel>
{
    public IList<AuditLogViewModel> AuditLogList { get; set; } = new List<AuditLogViewModel>();
    public TicketViewModel Ticket { get; set; } = new();
    public async Task<IActionResult> OnGet(string? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        _ = (await Mediatr.Send(new GetTicketByIdQuery(id))).Select(l=> Mapper.Map(l, Ticket));  
        AuditLogList = await Mediatr.Send(new GetAuditLogsByPrimaryKeyQuery(id));
        return Page();
    }
}
