using Emite.Common.Core.Base.Models;
using System.ComponentModel;

namespace Emite.CCM.Application.DTOs;

public record AgentListDto : BaseDto
{
	public string Email { get; init; } = "";
	public string Name { get; init; } = "";
	public string PhoneExtension { get; init; } = "";
	public string Status { get; init; } = "";
	
	
}
