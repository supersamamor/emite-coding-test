namespace Emite.CCM.Application.DTOs
{
    public record CallsPerAgentListDto
    {
        public string AgentName { get; set; } = "";
        public string AgentEmail { get; set; } = "";
        public int CallCount { get; set;} = 0;
    }
}
