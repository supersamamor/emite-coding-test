using Emite.CCM.Application.Helpers;
using Emite.CCM.Web.Areas.Admin.Queries.Entities;
using Emite.CCM.Web.Helper;
using Emite.CCM.Web.Models;
using DataTables.AspNetCore.Mvc.Binder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Emite.CCM.Web.Areas.Admin.Pages.BatchUploadJobs;
[Authorize(Policy = Permission.Admin.View)]
public class IndexModel : BasePageModel<IndexModel>
{
    [DataTablesRequest]
    public DataTablesRequest? DataRequest { get; set; }
    public IActionResult OnGet()
    {
        return Page();
    }
    public async Task<IActionResult> OnPostListAllAsync()
    {
        var result = await Mediatr.Send(DataRequest!.ToQuery<GetBatchUploadJobsQuery>());
        return new JsonResult(result.Data
            .Select(e => new
            {
                e.Id,
                e.Module,
                e.FileType,
                e.StartDateTime,
                FormattedStartDateTime = e.StartDateTime == null ? "" : e.StartDateTime.Value.ApplyTimeOffset().ToString("MM/dd/yyyy hh:mm:ss tt"),
                e.Duration,
                e.Remarks,
                Status = FileUploadStatusHelper.GetBadge(e.Status),
                e.LastModifiedDate,
                e.FormattedModule

            })
            .ToDataTablesResponse(DataRequest, result.TotalCount, result.MetaData.TotalItemCount));
    }
}
