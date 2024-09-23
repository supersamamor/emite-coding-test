using Emite.Common.Data;
using Emite.Common.Utility.Validators;
using Emite.CCM.Core.CCM;
using Emite.CCM.Infrastructure.Data;
using FluentValidation;
using LanguageExt;
using LanguageExt.Common;
using MediatR;
using static LanguageExt.Prelude;

namespace Emite.CCM.Application.Features.CCM.Ticket.Commands;

public record AssignTicketToAgentCommand : IRequest<Validation<Error, TicketState>>
{
    public string Id { get; set; } = "";
    public string AgentId { get; set; } = "";
}

public class AssignTicketToAgentCommandHandler(ApplicationContext context,
                                 CompositeValidator<AssignTicketToAgentCommand> validator) : IRequestHandler<AssignTicketToAgentCommand, Validation<Error, TicketState>>
{

    public async Task<Validation<Error, TicketState>> Handle(AssignTicketToAgentCommand request, CancellationToken cancellationToken) =>
            await validator.ValidateTAsync(request, cancellationToken).BindT(
                async request => await UpdateAgenStatus(request, cancellationToken));

    private async Task<Validation<Error, TicketState>> UpdateAgenStatus(AssignTicketToAgentCommand request, CancellationToken cancellationToken = default)
        => await context.GetSingle<TicketState>(e => e.Id == request.Id, tracking: true, cancellationToken: cancellationToken).MatchAsync(
            Some: async entity =>
            {
                entity.AssignToAgent(request.AgentId);
                context.Update(entity);
                _ = await context.SaveChangesAsync(cancellationToken);
                return Success<Error, TicketState>(entity);
            },
            None: () => Fail<Error, TicketState>($"Record with id {request.Id} does not exist"));
}

public class AssignTicketToAgentCommandValidator : AbstractValidator<AssignTicketToAgentCommand>
{
    readonly ApplicationContext _context;

    public AssignTicketToAgentCommandValidator(ApplicationContext context)
    {
        _context = context;
        RuleFor(x => x.Id).MustAsync(async (id, cancellation) => await _context.Exists<TicketState>(x => x.Id == id, cancellationToken: cancellation))
                          .WithMessage("Ticket with id {PropertyValue} does not exists");
    }
}
