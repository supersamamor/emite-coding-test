using Emite.Common.Utility.Models;
using Emite.CCM.Application.Features.CCM.Customer.Commands;
using Emite.CCM.Application.Features.CCM.Customer.Queries;
using Emite.CCM.Core.CCM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Emite.Common.API.Controllers;
using Asp.Versioning;
using Emite.CCM.Application.DTOs;

namespace Emite.CCM.API.Controllers.v1;

[ApiVersion("1.0")]
public class CustomerController : BaseApiController<CustomerController>
{
    [Authorize(Policy = Permission.Customer.View)]
    [HttpGet]
    public async Task<ActionResult<PagedListResponse<CustomerListDto>>> GetAsync([FromQuery] GetCustomerQuery query) =>
        Ok(await Mediator.Send(query));

    [Authorize(Policy = Permission.Customer.View)]
    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerState>> GetAsync(string id) =>
        await ToActionResult(async () => await Mediator.Send(new GetCustomerByIdQuery(id)));

    [Authorize(Policy = Permission.Customer.Create)]
    [HttpPost]
    public async Task<ActionResult<CustomerState>> PostAsync([FromBody] CustomerViewModel request) =>
        await ToActionResult(async () => await Mediator.Send(Mapper.Map<AddCustomerCommand>(request)));

    [Authorize(Policy = Permission.Customer.Edit)]
    [HttpPut("{id}")]
    public async Task<ActionResult<CustomerState>> PutAsync(string id, [FromBody] CustomerViewModel request)
    {
        var command = Mapper.Map<EditCustomerCommand>(request);
        return await ToActionResult(async () => await Mediator.Send(command with { Id = id }));
    }

    [Authorize(Policy = Permission.Customer.Delete)]
    [HttpDelete("{id}")]
    public async Task<ActionResult<CustomerState>> DeleteAsync(string id) =>
        await ToActionResult(async () => await Mediator.Send(new DeleteCustomerCommand { Id = id }));
}

public record CustomerViewModel
{
    [Required]
	[StringLength(100, ErrorMessage = "{0} length can't be more than {1}.")]
	public string Email { get;set; } = "";
	public DateTime? LastContactDate { get;set; } = DateTime.Now;
	[Required]
	[StringLength(100, ErrorMessage = "{0} length can't be more than {1}.")]
	public string Name { get;set; } = "";
	[Required]
	[StringLength(20, ErrorMessage = "{0} length can't be more than {1}.")]
	public string PhoneNumber { get;set; } = "";
	   
}
