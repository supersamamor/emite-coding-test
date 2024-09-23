using Emite.Common.Data;
using Emite.Common.Utility.Validators;
using Emite.CCM.Core.CCM;
using Emite.CCM.Infrastructure.Data;
using FluentValidation;
using LanguageExt;
using LanguageExt.Common;
using MediatR;
using static LanguageExt.Prelude;

namespace Emite.CCM.Application.Features.CCM.Agent.Commands;

public record UpdateAgentStatusCommand : IRequest<Validation<Error, AgentState>>
{
    public string Id { get; set; } = "";
    public string Status { get; set; } = "";
}

public class UpdateAgentStatusCommandHandler(ApplicationContext context,
                                 CompositeValidator<UpdateAgentStatusCommand> validator) : IRequestHandler<UpdateAgentStatusCommand, Validation<Error, AgentState>>
{

    public async Task<Validation<Error, AgentState>> Handle(UpdateAgentStatusCommand request, CancellationToken cancellationToken) =>
            await validator.ValidateTAsync(request, cancellationToken).BindT(
                async request => await UpdateAgenStatus(request, cancellationToken));

    private async Task<Validation<Error, AgentState>> UpdateAgenStatus(UpdateAgentStatusCommand request, CancellationToken cancellationToken = default)
        => await context.GetSingle<AgentState>(e => e.Id == request.Id, tracking: true, cancellationToken: cancellationToken).MatchAsync(
            Some: async entity =>
            {
                entity.TagStatus(request.Status);
                context.Update(entity);
                _ = await context.SaveChangesAsync(cancellationToken);
                return Success<Error, AgentState>(entity);
            },
            None: () => Fail<Error, AgentState>($"Record with id {request.Id} does not exist"));
}

public class UpdateAgentStatusCommandValidator : AbstractValidator<UpdateAgentStatusCommand>
{
    readonly ApplicationContext _context;

    public UpdateAgentStatusCommandValidator(ApplicationContext context)
    {
        _context = context;
        RuleFor(x => x.Id).MustAsync(async (id, cancellation) => await _context.Exists<AgentState>(x => x.Id == id, cancellationToken: cancellation))
                          .WithMessage("Agent with id {PropertyValue} does not exists");
        var listOfStatus = Common.Utility.Helpers.ConstantHelper.GetConstantValues<AgentStatus>();
        RuleFor(x => x.Status)
            .Must(status => listOfStatus.Contains(status))
            .WithMessage("Status '{PropertyValue}' is not valid. Allowed values are: " + string.Join(", ", listOfStatus));
    }
}
