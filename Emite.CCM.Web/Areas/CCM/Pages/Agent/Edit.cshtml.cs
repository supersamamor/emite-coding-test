using Emite.CCM.Application.Features.CCM.Agent.Commands;
using Emite.CCM.Application.Features.CCM.Agent.Queries;
using Emite.CCM.Web.Areas.CCM.Models;
using Emite.CCM.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Emite.CCM.Web.Areas.CCM.Pages.Agent;

[Authorize(Policy = Permission.Agent.Edit)]
public class EditModel : BasePageModel<EditModel>
{
    [BindProperty]
    public AgentViewModel Agent { get; set; } = new();
    [BindProperty]
    public string? RemoveSubDetailId { get; set; }
    [BindProperty]
    public string? AsyncAction { get; set; }
    public async Task<IActionResult> OnGet(string? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        return await PageFrom(async () => await Mediatr.Send(new GetAgentByIdQuery(id)), Agent);
    }

    public async Task<IActionResult> OnPost()
    {		
        if (!ModelState.IsValid)
        {
            return Page();
        }
		
        return await TryThenRedirectToPage(async () => await Mediatr.Send(Mapper.Map<EditAgentCommand>(Agent)), "Details", true);
    }	
	public IActionResult OnPostChangeFormValue()
    {
        ModelState.Clear();
		
        return Partial("_InputFieldsPartial", Agent);
    }
	
}
