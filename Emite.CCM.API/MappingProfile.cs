using AutoMapper;
using Emite.CCM.API.Controllers.v1;
using Emite.CCM.Application.Features.CCM.Agent.Commands;
using Emite.CCM.Application.Features.CCM.Customer.Commands;
using Emite.CCM.Application.Features.CCM.Call.Commands;
using Emite.CCM.Application.Features.CCM.Ticket.Commands;


namespace Emite.CCM.API;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<AgentViewModel, AddAgentCommand>();
		CreateMap <AgentViewModel, EditAgentCommand>();
		CreateMap<CustomerViewModel, AddCustomerCommand>();
		CreateMap <CustomerViewModel, EditCustomerCommand>();
		CreateMap<CallViewModel, AddCallCommand>();
		CreateMap <CallViewModel, EditCallCommand>();
		CreateMap<TicketViewModel, AddTicketCommand>();
		CreateMap <TicketViewModel, EditTicketCommand>();
		
    }
}
