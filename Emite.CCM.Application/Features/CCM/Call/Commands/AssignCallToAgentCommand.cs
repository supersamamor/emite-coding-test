using Emite.Common.Data;
using Emite.Common.Utility.Validators;
using Emite.CCM.Core.CCM;
using Emite.CCM.Infrastructure.Data;
using FluentValidation;
using LanguageExt;
using LanguageExt.Common;
using MediatR;
using static LanguageExt.Prelude;
using Microsoft.EntityFrameworkCore;

namespace Emite.CCM.Application.Features.CCM.Call.Commands;

public record AssignCallToAgentCommand : IRequest<Validation<Error, CallState>>
{
    public string Id { get; set; } = "";
    public string? AgentId { get; set; } = "";
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
                if (!string.IsNullOrEmpty(request.AgentId))
                {
                    entity.AssignToAgent(request.AgentId);
                }
                else
                {
                    entity.AssignToAgent(await GetAvailableAgent(cancellationToken));
                }
                context.Update(entity);
                _ = await context.SaveChangesAsync(cancellationToken);
                return Success<Error, CallState>(entity);
            },
            None: () => Fail<Error, CallState>($"Record with id {request.Id} does not exist"));

    private async Task<string?> GetAvailableAgent(CancellationToken cancellationToken)
    {
        var minCallCount = await context.Agent
            .MinAsync(a => a.CallList!.Count, cancellationToken);
        return await context.Agent
            .Where(a => a.CallList!.Count == minCallCount && a.Status == AgentStatus.Available).Select(l => l.Id)
            .FirstOrDefaultAsync(cancellationToken);
    }
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
