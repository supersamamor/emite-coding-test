using Emite.Common.Core.Base.Models;
using System.ComponentModel;

namespace Emite.CCM.Application.DTOs;

public record CustomerListDto : BaseDto
{
	public string Email { get; init; } = "";
	public DateTime? LastContactDate { get; init; }
	public string LastContactDateFormatted { get { return this.LastContactDate == null ? "" : this.LastContactDate!.Value.ToString("MMM dd, yyyy HH:mm"); } }
	public string Name { get; init; } = "";
	public string PhoneNumber { get; init; } = "";
	
	
}
