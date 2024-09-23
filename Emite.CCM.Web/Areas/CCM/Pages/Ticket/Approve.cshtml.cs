using Emite.CCM.Application.Features.CCM.Approval.Commands;
using Emite.CCM.Application.Features.CCM.Approval.Queries;
using Emite.CCM.Application.Features.CCM.Ticket.Queries;
using Emite.CCM.Web.Areas.CCM.Models;
using Emite.CCM.Web.Models;
using Emite.CCM.Core.CCM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Emite.CCM.Web.Areas.CCM.Pages.Ticket;

[Authorize(Policy = Permission.Ticket.Approve)]
public class ApproveModel : BasePageModel<ApproveModel>
{
    [BindProperty]
    public TicketViewModel Ticket { get; set; } = new();
    [BindProperty]
    public string? ApprovalStatus { get; set; }
	[BindProperty]
	public string? ApprovalRemarks { get; set; }
    public async Task<IActionResult> OnGet(string? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        _ = (await Mediatr.Send(new GetApprovalStatusPerApproverByIdQuery(id, ApprovalModule.Ticket))).Select(l => ApprovalStatus = l);
        return await PageFrom(async () => await Mediatr.Send(new GetTicketByIdQuery(id)), Ticket);
    }

    public async Task<IActionResult> OnPost(string handler)
    {
        if (handler == "Approve")
        {
            return await Approve();
        }
        else if (handler == "Reject")
        {
            return await Reject();
        }
        return Page();
    }
    private async Task<IActionResult> Approve()
    {
        return await TryThenRedirectToPage(async () => await Mediatr.Send(new ApproveCommand(Ticket.Id, ApprovalRemarks, ApprovalModule.Ticket)), "Approve", true);
    }
    private async Task<IActionResult> Reject()
    {
        return await TryThenRedirectToPage(async () => await Mediatr.Send(new RejectCommand(Ticket.Id, ApprovalRemarks, ApprovalModule.Ticket)), "Approve", true);
    }
}
