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
using static LanguageExt.Prelude;
using Emite.CCM.Application.Services;

namespace Emite.CCM.Application.Features.CCM.Ticket.Commands;

public record DeleteTicketCommand : BaseCommand, IRequest<Validation<Error, TicketState>>;

public class DeleteTicketCommandHandler(ApplicationContext context,
                                   IMapper mapper,
                                   CompositeValidator<DeleteTicketCommand> validator,
                                   ElasticSearchService? elasticSearchService) : BaseCommandHandler<ApplicationContext, TicketState, DeleteTicketCommand>(context, mapper, validator), IRequestHandler<DeleteTicketCommand, Validation<Error, TicketState>>
{
    public async Task<Validation<Error, TicketState>> Handle(DeleteTicketCommand request, CancellationToken cancellationToken) =>
        await Validators.ValidateTAsync(request, cancellationToken).BindT(
            async request => await DeleteTicket(request.Id, cancellationToken));

    private async Task<Validation<Error, TicketState>> DeleteTicket(string id, CancellationToken cancellationToken = default) =>
    await Context.GetSingle<TicketState>(e => e.Id == id, cancellationToken: cancellationToken).MatchAsync(
        Some: async entity =>
        {
            Context.Remove(entity);
            _ = await Context.SaveChangesAsync(cancellationToken);
            if (elasticSearchService != null)
            {
                await elasticSearchService.DeleteTicketAsync(entity.Id);
            }
            return Success<Error, TicketState>(entity);
        },
        None: () => Fail<Error, TicketState>($"Record with id {id} does not exist"));
}


public class DeleteTicketCommandValidator : AbstractValidator<DeleteTicketCommand>
{
    readonly ApplicationContext _context;

    public DeleteTicketCommandValidator(ApplicationContext context)
    {
        _context = context;
        RuleFor(x => x.Id).MustAsync(async (id, cancellation) => await _context.Exists<TicketState>(x => x.Id == id, cancellationToken: cancellation))
                          .WithMessage("Ticket with id {PropertyValue} does not exists");
    }
}
