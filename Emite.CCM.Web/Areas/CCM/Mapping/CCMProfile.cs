using AutoMapper;
using Emite.CCM.Core.CCM;
using Emite.CCM.Web.Areas.CCM.Models;
using Emite.CCM.Application.Features.CCM.Report.Commands;
using Emite.CCM.Application.DTOs;
using Emite.CCM.Application.Features.CCM.Approval.Commands;
using Emite.CCM.Application.Features.CCM.Agent.Commands;
using Emite.CCM.Application.Features.CCM.Customer.Commands;
using Emite.CCM.Application.Features.CCM.Call.Commands;
using Emite.CCM.Application.Features.CCM.Ticket.Commands;


namespace Emite.CCM.Web.Areas.CCM.Mapping;

public class CCMProfile : Profile
{
    public CCMProfile()
    {
		CreateMap<ReportViewModel, AddReportCommand>()
            .ForMember(dest => dest.ReportRoleAssignmentList, opt => opt.MapFrom(src => src.ReportRoleAssignmentList!.Select(x => new ReportRoleAssignmentState { RoleName = x })));
        CreateMap<ReportViewModel, EditReportCommand>()
           .ForMember(dest => dest.ReportRoleAssignmentList, opt => opt.MapFrom(src => src.ReportRoleAssignmentList!.Select(x => new ReportRoleAssignmentState { RoleName = x })));
        CreateMap<ReportState, ReportViewModel>()
            .ForMember(dest => dest.ReportRoleAssignmentList, opt => opt.MapFrom(src => src.ReportRoleAssignmentList!.Select(x => x.RoleName)));
        CreateMap<ReportViewModel, ReportState>();      
        CreateMap<ReportQueryFilterState, ReportQueryFilterViewModel>().ForPath(e => e.ForeignKeyReport, o => o.MapFrom(s => s.Report!.ReportName));
        CreateMap<ReportQueryFilterViewModel, ReportQueryFilterState>();
        CreateMap<ReportResultModel, ReportResultViewModel>().ReverseMap();
        CreateMap<ReportQueryFilterModel, ReportQueryFilterViewModel>().ReverseMap();
		CreateMap<ReportViewModel, AddReportWithSQLQueryFromAICommand>();
		
        CreateMap<AgentViewModel, AddAgentCommand>();
		CreateMap<AgentViewModel, EditAgentCommand>();
		CreateMap<AgentState, AgentViewModel>().ReverseMap();
		CreateMap<CustomerViewModel, AddCustomerCommand>();
		CreateMap<CustomerViewModel, EditCustomerCommand>();
		CreateMap<CustomerState, CustomerViewModel>().ReverseMap();
		CreateMap<CallViewModel, AddCallCommand>();
		CreateMap<CallViewModel, EditCallCommand>();
		CreateMap<CallState, CallViewModel>().ForPath(e => e.ReferenceFieldAgentId, o => o.MapFrom(s => s.Agent!.Id)).ForPath(e => e.ReferenceFieldCustomerId, o => o.MapFrom(s => s.Customer!.Id));
		CreateMap<CallViewModel, CallState>();
		CreateMap<TicketViewModel, AddTicketCommand>();
		CreateMap<TicketViewModel, EditTicketCommand>();
		CreateMap<TicketState, TicketViewModel>().ForPath(e => e.ReferenceFieldAgentId, o => o.MapFrom(s => s.Agent!.Id)).ForPath(e => e.ReferenceFieldCustomerId, o => o.MapFrom(s => s.Customer!.Id));
		CreateMap<TicketViewModel, TicketState>();
		
		CreateMap<ApproverAssignmentState, ApproverAssignmentViewModel>().ReverseMap();
		CreateMap<ApproverSetupViewModel, EditApproverSetupCommand>();
		CreateMap<ApproverSetupViewModel, AddApproverSetupCommand>();
		CreateMap<ApproverSetupState, ApproverSetupViewModel>().ReverseMap();
    }
}
