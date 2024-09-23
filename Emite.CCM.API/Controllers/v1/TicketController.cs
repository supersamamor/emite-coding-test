using Emite.Common.Utility.Models;
using Emite.CCM.Application.Features.CCM.Ticket.Commands;
using Emite.CCM.Application.Features.CCM.Ticket.Queries;
using Emite.CCM.Core.CCM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Emite.Common.API.Controllers;
using Asp.Versioning;
using Emite.CCM.Application.DTOs;
using Emite.CCM.Application.Features.CCM.Call.Commands;

namespace Emite.CCM.API.Controllers.v1;

[ApiVersion("1.0")]
public class TicketController : BaseApiController<TicketController>
{
    [Authorize(Policy = Permission.Ticket.View)]
    [HttpGet]
    public async Task<ActionResult<PagedListResponse<TicketListDto>>> GetAsync([FromQuery] GetTicketQuery query) =>
        Ok(await Mediator.Send(query));

    [Authorize(Policy = Permission.Ticket.View)]
    [HttpGet("{id}")]
    public async Task<ActionResult<TicketState>> GetAsync(string id) =>
        await ToActionResult(async () => await Mediator.Send(new GetTicketByIdQuery(id)));

    [Authorize(Policy = Permission.Ticket.Create)]
    [HttpPost]
    public async Task<ActionResult<TicketState>> PostAsync([FromBody] TicketViewModel request) =>
        await ToActionResult(async () => await Mediator.Send(Mapper.Map<AddTicketCommand>(request)));

    [Authorize(Policy = Permission.Ticket.Edit)]
    [HttpPut("{id}")]
    public async Task<ActionResult<TicketState>> PutAsync(string id, [FromBody] TicketViewModel request)
    {
        var command = Mapper.Map<EditTicketCommand>(request);
        return await ToActionResult(async () => await Mediator.Send(command with { Id = id }));
    }

    [Authorize(Policy = Permission.Ticket.Delete)]
    [HttpDelete("{id}")]
    public async Task<ActionResult<TicketState>> DeleteAsync(string id) =>
        await ToActionResult(async () => await Mediator.Send(new DeleteTicketCommand { Id = id }));

    [Authorize(Policy = Permission.Ticket.AssignToAgent)]
    [HttpPatch("{id}")]
    public async Task<ActionResult<TicketState>> PatchAsync(string id, string agentId) =>
        await ToActionResult(async () => await Mediator.Send(new AssignTicketToAgentCommand { Id = id, AgentId = agentId }));
}

public record TicketViewModel
{

    public string? AgentId { get; set; }
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    [Required]
    [StringLength(50, ErrorMessage = "{0} length can't be more than {1}.")]
    public string CustomerId { get; set; } = "";
    [Required]

    public string Description { get; set; } = "";
    [Required]
    [StringLength(50, ErrorMessage = "{0} length can't be more than {1}.")]
    public string Priority { get; set; } = "";
    public string? Resolution { get; set; }
    [Required]
    [StringLength(50, ErrorMessage = "{0} length can't be more than {1}.")]
    public string Status { get; set; } = "";
    [Required]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

}
