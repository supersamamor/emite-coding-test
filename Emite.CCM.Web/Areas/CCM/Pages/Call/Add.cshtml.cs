using Emite.CCM.Application.Features.CCM.Call.Commands;
using Emite.CCM.Web.Areas.CCM.Models;
using Emite.CCM.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Emite.CCM.Web.Areas.CCM.Pages.Call;

[Authorize(Policy = Permission.Call.Create)]
public class AddModel : BasePageModel<AddModel>
{
    [BindProperty]
    public CallViewModel Call { get; set; } = new();
    [BindProperty]
    public string? RemoveSubDetailId { get; set; }
    [BindProperty]
    public string? AsyncAction { get; set; }
    public IActionResult OnGet()
    {
		
        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
		
        if (!ModelState.IsValid)
        {
            return Page();
        }
		
        return await TryThenRedirectToPage(async () => await Mediatr.Send(Mapper.Map<AddCallCommand>(Call)), "Details", true);
    }	
	public IActionResult OnPostChangeFormValue()
    {
        ModelState.Clear();
		
        return Partial("_InputFieldsPartial", Call);
    }
	
}
