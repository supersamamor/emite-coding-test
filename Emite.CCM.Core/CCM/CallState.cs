using Emite.Common.Core.Base.Models;
using System.ComponentModel;

namespace Emite.CCM.Core.CCM;

public record CallState : BaseEntity
{
    public string? AgentId { get; init; }
    public string CustomerId { get; init; } = "";
    public DateTime? EndTime { get; init; }
    public string Notes { get; init; } = "";
    public DateTime StartTime { get; init; }
    public string Status { get; init; } = CallStatus.Queued;

    public AgentState? Agent { get; set; }
    public CustomerState? Customer { get; set; }


}
public class CallStatus
{
    public const string Queued = "Queued";
    public const string InProgress = "In-Progress";
    public const string Completed = "Completed";
    public const string Dropped = "Dropped";
}