using Emite.Common.Core.Base.Models;

namespace Emite.CCM.Core.CCM;

public record AgentState : BaseEntity
{
    public string Email { get; init; } = "";
    public string Name { get; init; } = "";
    public string PhoneExtension { get; init; } = "";
    public string Status { get; private set; } = AgentStatus.Available;


    public IList<CallState>? CallList { get; set; }
    public IList<TicketState>? TicketList { get; set; }
    public void TagStatus(string status)
    {
        this.Status = Status;
    }

}
public class AgentStatus
{
    public const string Available = "Available";
    public const string Busy = "Busy";
    public const string Offline = "Offline";
}