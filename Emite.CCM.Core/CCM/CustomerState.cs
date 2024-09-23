using Emite.Common.Core.Base.Models;
using System.ComponentModel;

namespace Emite.CCM.Core.CCM;

public record CustomerState : BaseEntity
{
	public string Email { get; init; } = "";
	public DateTime? LastContactDate { get; init; }
	public string Name { get; init; } = "";
	public string PhoneNumber { get; init; } = "";
	
	
	public IList<CallState>? CallList { get; set; }
	public IList<TicketState>? TicketList { get; set; }
	
}
