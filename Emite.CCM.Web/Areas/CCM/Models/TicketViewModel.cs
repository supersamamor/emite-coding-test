using Emite.Common.Web.Utility.Extensions;
using Emite.CCM.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using Emite.Common.Web.Utility.Annotations;

namespace Emite.CCM.Web.Areas.CCM.Models;

public record TicketViewModel : BaseViewModel
{	
	[Display(Name = "Agent Id")]
	
	public string? AgentId { get; init; }
	public string?  ReferenceFieldAgentId { get; set; }
	[Display(Name = "Created At")]
	[Required]
	public DateTime CreatedAt { get; init; } = DateTime.Now;
	[Display(Name = "Customer Id")]
	[Required]
	[StringLength(50, ErrorMessage = "{0} length can't be more than {1}.")]
	public string CustomerId { get; init; } = "";
	public string?  ReferenceFieldCustomerId { get; set; }
	[Display(Name = "Description")]
	[Required]
	
	public string Description { get; init; } = "";
	[Display(Name = "Priority")]
	[Required]
	[StringLength(50, ErrorMessage = "{0} length can't be more than {1}.")]
	public string Priority { get; init; } = "";
	[Display(Name = "Resolution")]
	public string? Resolution { get; init; }
	[Display(Name = "Status")]
	[Required]
	[StringLength(50, ErrorMessage = "{0} length can't be more than {1}.")]
	public string Status { get; init; } = "";
	[Display(Name = "Updated At")]
	[Required]
	public DateTime UpdatedAt { get; init; } = DateTime.Now;
	
	public DateTime LastModifiedDate { get; set; }
	public AgentViewModel? Agent { get; init; }
	public CustomerViewModel? Customer { get; init; }
		
	
}
