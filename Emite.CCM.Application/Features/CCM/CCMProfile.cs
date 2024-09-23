using AutoMapper;
using Emite.Common.Core.Mapping;
using Emite.CCM.Application.Features.CCM.Approval.Commands;
using Emite.CCM.Core.CCM;
using Emite.CCM.Application.Features.CCM.Report.Commands;
using Emite.CCM.Application.Features.CCM.Agent.Commands;
using Emite.CCM.Application.Features.CCM.Customer.Commands;
using Emite.CCM.Application.Features.CCM.Call.Commands;
using Emite.CCM.Application.Features.CCM.Ticket.Commands;



namespace Emite.CCM.Application.Features.CCM;

public class CCMProfile : Profile
{
    public CCMProfile()
    {
		CreateMap<AddReportCommand, ReportState>();
        CreateMap<EditReportCommand, ReportState>().IgnoreBaseEntityProperties();
		CreateMap<AddReportAnalyticsCommand, ReportAIIntegrationState>();
		CreateMap<AddReportWithSQLQueryFromAICommand, ReportState>();
		
        CreateMap<AddAgentCommand, AgentState>();
		CreateMap <EditAgentCommand, AgentState>().IgnoreBaseEntityProperties();
		CreateMap<AddCustomerCommand, CustomerState>();
		CreateMap <EditCustomerCommand, CustomerState>().IgnoreBaseEntityProperties();
		CreateMap<AddCallCommand, CallState>();
		CreateMap <EditCallCommand, CallState>().IgnoreBaseEntityProperties();
		CreateMap<AddTicketCommand, TicketState>();
		CreateMap <EditTicketCommand, TicketState>().IgnoreBaseEntityProperties();
		
		CreateMap<EditApproverSetupCommand, ApproverSetupState>().IgnoreBaseEntityProperties();
		CreateMap<AddApproverSetupCommand, ApproverSetupState>().IgnoreBaseEntityProperties();
		CreateMap<ApproverAssignmentState, ApproverAssignmentState>().IgnoreBaseEntityProperties();
    }
}
