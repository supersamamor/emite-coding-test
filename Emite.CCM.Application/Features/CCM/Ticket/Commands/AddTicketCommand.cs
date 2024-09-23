using AutoMapper;
using Emite.Common.Core.Commands;
using Emite.Common.Data;
using Emite.Common.Utility.Validators;
using Emite.CCM.Core.CCM;
using Emite.CCM.Infrastructure.Data;
using FluentValidation;
using LanguageExt;
using LanguageExt.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static LanguageExt.Prelude;

namespace Emite.CCM.Application.Features.CCM.Ticket.Commands;

public record AddTicketCommand : TicketState, IRequest<Validation<Error, TicketState>>;

public class AddTicketCommandHandler(ApplicationContext context,
                                IMapper mapper,
                                CompositeValidator<AddTicketCommand> validator,
                                    IdentityContext identityContext) : BaseCommandHandler<ApplicationContext, TicketState, AddTicketCommand>(context, mapper, validator), IRequestHandler<AddTicketCommand, Validation<Error, TicketState>>
{
    public async Task<Validation<Error, TicketState>> Handle(AddTicketCommand request, CancellationToken cancellationToken) =>
		await Validators.ValidateTAsync(request, cancellationToken).BindT(
			async request => await AddTicket(request, cancellationToken));


	public async Task<Validation<Error, TicketState>> AddTicket(AddTicketCommand request, CancellationToken cancellationToken)
	{
		TicketState entity = Mapper.Map<TicketState>(request);
		_ = await Context.AddAsync(entity, cancellationToken);
		await Helpers.ApprovalHelper.AddApprovers(Context, identityContext, ApprovalModule.Ticket, entity.Id, cancellationToken);
		_ = await Context.SaveChangesAsync(cancellationToken);
		return Success<Error, TicketState>(entity);
	}
	
}

public class AddTicketCommandValidator : AbstractValidator<AddTicketCommand>
{
    readonly ApplicationContext _context;

    public AddTicketCommandValidator(ApplicationContext context)
    {
        _context = context;

        RuleFor(x => x.Id).MustAsync(async (id, cancellation) => await _context.NotExists<TicketState>(x => x.Id == id, cancellationToken: cancellation))
                          .WithMessage("Ticket with id {PropertyValue} already exists");
        
    }
}
