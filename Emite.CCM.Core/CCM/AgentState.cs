using Emite.Common.Core.Base.Models;
using System.ComponentModel;

namespace Emite.CCM.Core.CCM;

public record AgentState : BaseEntity
{
    public string Email { get; init; } = "";
    public string Name { get; init; } = "";
    public string PhoneExtension { get; init; } = "";
    public string Status { get; init; } = AgentStatus.Available;


    public IList<CallState>? CallList { get; set; }
    public IList<TicketState>? TicketList { get; set; }

}
public class AgentStatus
{
    public const string Available = "Available";
    public const string Busy = "Busy";
    public const string Offline = "Offline";
}