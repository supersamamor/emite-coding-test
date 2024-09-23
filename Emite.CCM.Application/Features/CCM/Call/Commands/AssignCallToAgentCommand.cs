using Emite.Common.Data;
using Emite.Common.Utility.Validators;
using Emite.CCM.Core.CCM;
using Emite.CCM.Infrastructure.Data;
using FluentValidation;
using LanguageExt;
using LanguageExt.Common;
using MediatR;
using static LanguageExt.Prelude;

namespace Emite.CCM.Application.Features.CCM.Call.Commands;

public record AssignCallToAgentCommand : IRequest<Validation<Error, CallState>>
{
    public string Id { get; set; } = "";
    public string AgentId { get; set; } = "";
}

public class AssignCallToAgentCommandHandler(ApplicationContext context,
                                 CompositeValidator<AssignCallToAgentCommand> validator) : IRequestHandler<AssignCallToAgentCommand, Validation<Error, CallState>>
{

    public async Task<Validation<Error, CallState>> Handle(AssignCallToAgentCommand request, CancellationToken cancellationToken) =>
            await validator.ValidateTAsync(request, cancellationToken).BindT(
                async request => await UpdateAgenStatus(request, cancellationToken));

    private async Task<Validation<Error, CallState>> UpdateAgenStatus(AssignCallToAgentCommand request, CancellationToken cancellationToken = default)
        => await context.GetSingle<CallState>(e => e.Id == request.Id, tracking: true, cancellationToken: cancellationToken).MatchAsync(
            Some: async entity =>
            {
                entity.AssignToAgent(request.AgentId);
                context.Update(entity);
                _ = await context.SaveChangesAsync(cancellationToken);
                return Success<Error, CallState>(entity);
            },
            None: () => Fail<Error, CallState>($"Record with id {request.Id} does not exist"));
}

public class AssignCallToAgentCommandValidator : AbstractValidator<AssignCallToAgentCommand>
{
    readonly ApplicationContext _context;

    public AssignCallToAgentCommandValidator(ApplicationContext context)
    {
        _context = context;
        RuleFor(x => x.Id).MustAsync(async (id, cancellation) => await _context.Exists<CallState>(x => x.Id == id, cancellationToken: cancellation))
                          .WithMessage("Call with id {PropertyValue} does not exists");
    }
}
