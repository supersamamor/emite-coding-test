using Emite.Common.Web.Utility.Extensions;
using Emite.CCM.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using Emite.Common.Web.Utility.Annotations;

namespace Emite.CCM.Web.Areas.CCM.Models;

public record AgentViewModel : BaseViewModel
{	
	[Display(Name = "Email")]
	[Required]
	[StringLength(100, ErrorMessage = "{0} length can't be more than {1}.")]
	public string Email { get; init; } = "";
	[Display(Name = "Name")]
	[Required]
	[StringLength(100, ErrorMessage = "{0} length can't be more than {1}.")]
	public string Name { get; init; } = "";
	[Display(Name = "Phone Extension")]
	[Required]
	[StringLength(50, ErrorMessage = "{0} length can't be more than {1}.")]
	public string PhoneExtension { get; init; } = "";
	[Display(Name = "Status")]
	[Required]
	[StringLength(50, ErrorMessage = "{0} length can't be more than {1}.")]
	public string Status { get; init; } = "";
	
	public DateTime LastModifiedDate { get; set; }
		
	public IList<CallViewModel>? CallList { get; set; }
	public IList<TicketViewModel>? TicketList { get; set; }
	
}
