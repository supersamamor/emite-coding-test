using Emite.CCM.Application.Features.CCM.Agent.Queries;
using Emite.CCM.Core.CCM;
using Emite.CCM.Web.Models;
using Emite.CCM.ExcelProcessor.Services;
using DataTables.AspNetCore.Mvc.Binder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Emite.CCM.Web.Areas.CCM.Pages.Agent;

[Authorize(Policy = Permission.Agent.View)]
public class IndexModel : BasePageModel<IndexModel>
{   
	private readonly string? _uploadPath;
    public IndexModel(IConfiguration configuration)
    {
        _uploadPath = configuration.GetValue<string>("UsersUpload:UploadFilesPath");
    }
    [DataTablesRequest]
    public DataTablesRequest? DataRequest { get; set; }
	[BindProperty]
    public BatchUploadModel BatchUpload { get; set; } = new();
    public IActionResult OnGet()
    {
        return Page();
    }

    public async Task<IActionResult> OnPostListAllAsync()
    {
		var result = await Mediatr.Send(DataRequest!.ToQuery<GetAgentQuery>());
		return new JsonResult(result.Data.ToDataTablesResponse(DataRequest, result.TotalCount, result.MetaData.TotalItemCount));	
    } 
	
	public async Task<IActionResult> OnGetSelect2Data([FromQuery] Select2Request request)
    {
        var result = await Mediatr.Send(request.ToQuery<GetAgentQuery>(nameof(AgentState.Id)));
        return new JsonResult(result.ToSelect2Response(e => new Select2Result { Id = e.Id, Text = e.Name }));
    }
	public async Task<IActionResult> OnPostBatchUploadAsync()
    {        
        return await BatchUploadAsync<IndexModel, AgentState>(BatchUpload.BatchUploadForm, nameof(AgentState), "Index");
    }

    public IActionResult OnPostDownloadTemplate()
    {
        ModelState.Clear();
		BatchUpload.BatchUploadFileName = ExcelService.ExportTemplate<AgentState>(_uploadPath + "\\" + WebConstants.ExcelTemplateSubFolder);
        NotyfService.Success(Localizer["Successfully downloaded upload template."]);
        return Page();
    }
}
