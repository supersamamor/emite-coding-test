using Microsoft.AspNetCore.Mvc;
using Emite.Common.API.Controllers;
using Asp.Versioning;
using Emite.CCM.Application.Features.CCM.AverageCallDuration.Queries;
using Emite.CCM.Application.Features.CCM.CallsPerAgent.Queries;
using Microsoft.AspNetCore.Authorization;

namespace Emite.CCM.API.Controllers.v1;

[ApiVersion("1.0")]
public class DashboardController : BaseApiController<DashboardController>
{
    [AllowAnonymous]
    [HttpGet("AverageCallDuration")]
    public async Task<ActionResult<double?>> GetAsync([FromQuery] GetAverageCallDurationQuery query) =>
        Ok(await Mediator.Send(query));

    [AllowAnonymous]
    [HttpGet("CallsPerAgent")]
    public async Task<ActionResult<double?>> GetAsync([FromQuery] GetCallsPerAgentQuery query) =>
       Ok(await Mediator.Send(query));

}
