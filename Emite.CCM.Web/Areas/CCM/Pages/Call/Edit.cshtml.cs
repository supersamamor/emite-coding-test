using Emite.CCM.Application.Features.CCM.Call.Commands;
using Emite.CCM.Application.Features.CCM.Call.Queries;
using Emite.CCM.Web.Areas.CCM.Models;
using Emite.CCM.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Emite.CCM.Web.Areas.CCM.Pages.Call;

[Authorize(Policy = Permission.Call.Edit)]
public class EditModel : BasePageModel<EditModel>
{
    [BindProperty]
    public CallViewModel Call { get; set; } = new();
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
        return await PageFrom(async () => await Mediatr.Send(new GetCallByIdQuery(id)), Call);
    }

    public async Task<IActionResult> OnPost()
    {		
        if (!ModelState.IsValid)
        {
            return Page();
        }
		
        return await TryThenRedirectToPage(async () => await Mediatr.Send(Mapper.Map<EditCallCommand>(Call)), "Details", true);
    }	
	public IActionResult OnPostChangeFormValue()
    {
        ModelState.Clear();
		
        return Partial("_InputFieldsPartial", Call);
    }
	
}
