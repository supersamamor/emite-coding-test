using Emite.Common.Core.Base.Models;
namespace Emite.CCM.Core.CCM;
public record TicketState : BaseEntity
{
    public string? AgentId { get; private set; }
    public DateTime CreatedAt { get; init; }
    public string CustomerId { get; init; } = "";
    public string Description { get; init; } = "";
    public string Priority { get; init; } = TicketPrioritization.Low;
    public string? Resolution { get; init; }
    public string Status { get; private set; } = TicketStatus.Open;
    public DateTime UpdatedAt { get; init; }
    public AgentState? Agent { get; set; }
    public CustomerState? Customer { get; set; }
    public void AssignToAgent(string agentId)
    {
        this.AgentId = agentId;
    }
}
public class TicketPrioritization
{
    public const string Low = "Low";
    public const string Medium = "Medium";
    public const string High = "High";
    public const string Urgent = "Urgent";
}
public class TicketStatus
{
    public const string Open = "Open";
    public const string InProgress = "In Progress";
    public const string Resolved = "Resolved";
    public const string Closed = "Closed";
}