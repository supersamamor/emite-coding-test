using Emite.Common.Web.Utility.Extensions;
using Emite.CCM.Web.Areas.Admin.Models;
using Emite.CCM.Web.Areas.Admin.Queries.Entities;
using Emite.CCM.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Emite.CCM.Web.Areas.Admin.Pages.Entities;

[Authorize(Policy = Permission.Entities.View)]
public class ViewModel : BasePageModel<ViewModel>
{
    public EntityViewModel Entity { get; set; } = new();

    public async Task<IActionResult> OnGet(string? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        return await Mediatr.Send(new GetEntityByIdQuery(id)).ToActionResult(
            e =>
            {
                Mapper.Map(e, Entity);
                return Page();
            }, none: null);
    }
}
