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

public record EditTicketCommand : TicketState, IRequest<Validation<Error, TicketState>>;

public class EditTicketCommandHandler(ApplicationContext context,
                                 IMapper mapper,
                                 CompositeValidator<EditTicketCommand> validator) : BaseCommandHandler<ApplicationContext, TicketState, EditTicketCommand>(context, mapper, validator), IRequestHandler<EditTicketCommand, Validation<Error, TicketState>>
{ 
    
public async Task<Validation<Error, TicketState>> Handle(EditTicketCommand request, CancellationToken cancellationToken) =>
		await Validators.ValidateTAsync(request, cancellationToken).BindT(
			async request => await Edit(request, cancellationToken));
	
}

public class EditTicketCommandValidator : AbstractValidator<EditTicketCommand>
{
    readonly ApplicationContext _context;

    public EditTicketCommandValidator(ApplicationContext context)
    {
        _context = context;
		RuleFor(x => x.Id).MustAsync(async (id, cancellation) => await _context.Exists<TicketState>(x => x.Id == id, cancellationToken: cancellation))
                          .WithMessage("Ticket with id {PropertyValue} does not exists");
        
    }
}
