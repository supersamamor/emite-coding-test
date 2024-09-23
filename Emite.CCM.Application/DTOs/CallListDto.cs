using Emite.Common.Core.Base.Models;
using System.ComponentModel;

namespace Emite.CCM.Application.DTOs;

public record CallListDto : BaseDto
{
	public string? AgentId { get; init; }
	public string CustomerId { get; init; } = "";
	public DateTime? EndTime { get; init; }
	public string EndTimeFormatted { get { return this.EndTime == null ? "" : this.EndTime!.Value.ToString("MMM dd, yyyy HH:mm"); } }
	public string Notes { get; init; } = "";
	public DateTime StartTime { get; init; }
	public string StartTimeFormatted { get { return this.StartTime.ToString("MMM dd, yyyy HH:mm"); } }
	public string Status { get; init; } = "";
	
	
}
