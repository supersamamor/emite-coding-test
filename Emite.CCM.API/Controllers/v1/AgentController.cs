using Emite.Common.Utility.Models;
using Emite.CCM.Application.Features.CCM.Agent.Commands;
using Emite.CCM.Application.Features.CCM.Agent.Queries;
using Emite.CCM.Core.CCM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Emite.Common.API.Controllers;
using Asp.Versioning;
using Emite.CCM.Application.DTOs;

namespace Emite.CCM.API.Controllers.v1;

[ApiVersion("1.0")]
public class AgentController : BaseApiController<AgentController>
{
    [Authorize(Policy = Permission.Agent.View)]
    [HttpGet]
    public async Task<ActionResult<PagedListResponse<AgentListDto>>> GetAsync([FromQuery] GetAgentQuery query) =>
        Ok(await Mediator.Send(query));

    [Authorize(Policy = Permission.Agent.View)]
    [HttpGet("{id}")]
    public async Task<ActionResult<AgentState>> GetAsync(string id) =>
        await ToActionResult(async () => await Mediator.Send(new GetAgentByIdQuery(id)));

    [Authorize(Policy = Permission.Agent.Create)]
    [HttpPost]
    public async Task<ActionResult<AgentState>> PostAsync([FromBody] AgentViewModel request) =>
        await ToActionResult(async () => await Mediator.Send(Mapper.Map<AddAgentCommand>(request)));

    [Authorize(Policy = Permission.Agent.Edit)]
    [HttpPut("{id}")]
    public async Task<ActionResult<AgentState>> PutAsync(string id, [FromBody] AgentViewModel request)
    {
        var command = Mapper.Map<EditAgentCommand>(request);
        return await ToActionResult(async () => await Mediator.Send(command with { Id = id }));
    }

    [Authorize(Policy = Permission.Agent.Delete)]
    [HttpDelete("{id}")]
    public async Task<ActionResult<AgentState>> DeleteAsync(string id) =>
        await ToActionResult(async () => await Mediator.Send(new DeleteAgentCommand { Id = id }));

    [Authorize(Policy = Permission.Agent.UpdateStatus)]
    [HttpPatch("{id}")]
    public async Task<ActionResult<AgentState>> PatchAsync(string id, string status) =>
        await ToActionResult(async () => await Mediator.Send(new UpdateAgentStatusCommand { Id = id, Status = status }));
}

public record AgentViewModel
{
    [Required]
	[StringLength(100, ErrorMessage = "{0} length can't be more than {1}.")]
	public string Email { get;set; } = "";
	[Required]
	[StringLength(100, ErrorMessage = "{0} length can't be more than {1}.")]
	public string Name { get;set; } = "";
	[Required]
	[StringLength(50, ErrorMessage = "{0} length can't be more than {1}.")]
	public string PhoneExtension { get;set; } = "";
	[Required]
	[StringLength(50, ErrorMessage = "{0} length can't be more than {1}.")]
	public string Status { get;set; } = "";
	   
}
