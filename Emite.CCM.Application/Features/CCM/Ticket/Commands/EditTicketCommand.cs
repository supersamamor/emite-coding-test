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
using Emite.CCM.Application.Services;

namespace Emite.CCM.Application.Features.CCM.Ticket.Commands;

public record EditTicketCommand : TicketState, IRequest<Validation<Error, TicketState>>;

public class EditTicketCommandHandler(ApplicationContext context,
                                 IMapper mapper,
                                 CompositeValidator<EditTicketCommand> validator,
                                 ElasticSearchService? elasticSearchService) : BaseCommandHandler<ApplicationContext, TicketState, EditTicketCommand>(context, mapper, validator), IRequestHandler<EditTicketCommand, Validation<Error, TicketState>>
{

    public async Task<Validation<Error, TicketState>> Handle(EditTicketCommand request, CancellationToken cancellationToken) =>
            await Validators.ValidateTAsync(request, cancellationToken).BindT(
                async request => await EditTicket(request, cancellationToken));

    private async Task<Validation<Error, TicketState>> EditTicket(EditTicketCommand request, CancellationToken cancellationToken = default) =>
        await Context.GetSingle<TicketState>(e => e.Id == request.Id, tracking: true, cancellationToken: cancellationToken).MatchAsync(
            Some: async entity =>
            {
                Mapper.Map(request, entity);
                Context.Update(entity);
                _ = await Context.SaveChangesAsync(cancellationToken);
                if (elasticSearchService != null)
                {
                    await elasticSearchService.UpdateTicketAsync(entity);
                }
                return Success<Error, TicketState>(entity);
            },
            None: () => Fail<Error, TicketState>($"Record with id {request.Id} does not exist"));
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
