using Emite.Common.Web.Utility.Extensions;
using Emite.CCM.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using Emite.Common.Web.Utility.Annotations;

namespace Emite.CCM.Web.Areas.CCM.Models;

public record CustomerViewModel : BaseViewModel
{	
	[Display(Name = "Email")]
	[Required]
	[StringLength(100, ErrorMessage = "{0} length can't be more than {1}.")]
	public string Email { get; init; } = "";
	[Display(Name = "Last Contact Date")]
	public DateTime? LastContactDate { get; init; } = DateTime.Now;
	[Display(Name = "Name")]
	[Required]
	[StringLength(100, ErrorMessage = "{0} length can't be more than {1}.")]
	public string Name { get; init; } = "";
	[Display(Name = "Phone Number")]
	[Required]
	[StringLength(20, ErrorMessage = "{0} length can't be more than {1}.")]
	public string PhoneNumber { get; init; } = "";
	
	public DateTime LastModifiedDate { get; set; }
		
	public IList<CallViewModel>? CallList { get; set; }
	public IList<TicketViewModel>? TicketList { get; set; }
	
}
