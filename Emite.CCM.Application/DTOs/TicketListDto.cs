using Emite.Common.Core.Base.Models;
using System.ComponentModel;

namespace Emite.CCM.Application.DTOs;

public record TicketListDto : BaseDto
{
	public string? AgentName { get; init; }
	public DateTime CreatedAt { get; init; }
	public string CreatedAtFormatted { get { return this.CreatedAt.ToString("MMM dd, yyyy HH:mm"); } }
	public string CustomerName { get; init; } = "";
	public string Description { get; init; } = "";
	public string Priority { get; init; } = "";
	public string? Resolution { get; init; }
	public string Status { get; init; } = "";
	public DateTime UpdatedAt { get; init; }
	public string UpdatedAtFormatted { get { return this.UpdatedAt.ToString("MMM dd, yyyy HH:mm"); } }
	
	public string StatusBadge { get; set; } = "";
}
