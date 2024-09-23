using Emite.Common.Utility.Models;
using Emite.CCM.Application.Features.CCM.Call.Commands;
using Emite.CCM.Application.Features.CCM.Call.Queries;
using Emite.CCM.Core.CCM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Emite.Common.API.Controllers;
using Asp.Versioning;

namespace Emite.CCM.API.Controllers.v1;

[ApiVersion("1.0")]
public class CallController : BaseApiController<CallController>
{
    [Authorize(Policy = Permission.Call.View)]
    [HttpGet]
    public async Task<ActionResult<PagedListResponse<CallState>>> GetAsync([FromQuery] GetCallQuery query) =>
        Ok(await Mediator.Send(query));

    [Authorize(Policy = Permission.Call.View)]
    [HttpGet("{id}")]
    public async Task<ActionResult<CallState>> GetAsync(string id) =>
        await ToActionResult(async () => await Mediator.Send(new GetCallByIdQuery(id)));

    [Authorize(Policy = Permission.Call.Create)]
    [HttpPost]
    public async Task<ActionResult<CallState>> PostAsync([FromBody] CallViewModel request) =>
        await ToActionResult(async () => await Mediator.Send(Mapper.Map<AddCallCommand>(request)));

    [Authorize(Policy = Permission.Call.Edit)]
    [HttpPut("{id}")]
    public async Task<ActionResult<CallState>> PutAsync(string id, [FromBody] CallViewModel request)
    {
        var command = Mapper.Map<EditCallCommand>(request);
        return await ToActionResult(async () => await Mediator.Send(command with { Id = id }));
    }

    [Authorize(Policy = Permission.Call.Delete)]
    [HttpDelete("{id}")]
    public async Task<ActionResult<CallState>> DeleteAsync(string id) =>
        await ToActionResult(async () => await Mediator.Send(new DeleteCallCommand { Id = id }));
}

public record CallViewModel
{
    
	public string? AgentId { get;set; }
	[Required]
	[StringLength(50, ErrorMessage = "{0} length can't be more than {1}.")]
	public string CustomerId { get;set; } = "";
	public DateTime? EndTime { get;set; } = DateTime.Now;
	[Required]
	public string Notes { get;set; } = "";
	[Required]
	public DateTime StartTime { get;set; } = DateTime.Now;
	[Required]
	[StringLength(50, ErrorMessage = "{0} length can't be more than {1}.")]
	public string Status { get;set; } = "";
	   
}
