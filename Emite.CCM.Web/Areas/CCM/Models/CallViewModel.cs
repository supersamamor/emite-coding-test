using Emite.Common.Web.Utility.Extensions;
using Emite.CCM.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using Emite.Common.Web.Utility.Annotations;

namespace Emite.CCM.Web.Areas.CCM.Models;

public record CallViewModel : BaseViewModel
{	
	[Display(Name = "Agent Id")]
	
	public string? AgentId { get; init; }
	public string?  ReferenceFieldAgentId { get; set; }
	[Display(Name = "Customer Id")]
	[Required]
	[StringLength(50, ErrorMessage = "{0} length can't be more than {1}.")]
	public string CustomerId { get; init; } = "";
	public string?  ReferenceFieldCustomerId { get; set; }
	[Display(Name = "End Time")]
	public DateTime? EndTime { get; init; } = DateTime.Now;
	[Display(Name = "Notes")]
	[Required]
	public string Notes { get; init; } = "";
	[Display(Name = "Start Time")]
	[Required]
	public DateTime StartTime { get; init; } = DateTime.Now;
	[Display(Name = "Status")]
	[Required]
	[StringLength(50, ErrorMessage = "{0} length can't be more than {1}.")]
	public string Status { get; init; } = "";
	
	public DateTime LastModifiedDate { get; set; }
	public AgentViewModel? Agent { get; init; }
	public CustomerViewModel? Customer { get; init; }
		
	
}
