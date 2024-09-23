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

namespace Emite.CCM.Application.Features.CCM.Agent.Commands;

public record DeleteAgentCommand : BaseCommand, IRequest<Validation<Error, AgentState>>;

public class DeleteAgentCommandHandler(ApplicationContext context,
                                   IMapper mapper,
                                   CompositeValidator<DeleteAgentCommand> validator) : BaseCommandHandler<ApplicationContext, AgentState, DeleteAgentCommand>(context, mapper, validator), IRequestHandler<DeleteAgentCommand, Validation<Error, AgentState>>
{ 
    public async Task<Validation<Error, AgentState>> Handle(DeleteAgentCommand request, CancellationToken cancellationToken) =>
        await Validators.ValidateTAsync(request, cancellationToken).BindT(
            async request => await Delete(request.Id, cancellationToken));
}


public class DeleteAgentCommandValidator : AbstractValidator<DeleteAgentCommand>
{
    readonly ApplicationContext _context;

    public DeleteAgentCommandValidator(ApplicationContext context)
    {
        _context = context;
        RuleFor(x => x.Id).MustAsync(async (id, cancellation) => await _context.Exists<AgentState>(x => x.Id == id, cancellationToken: cancellation))
                          .WithMessage("Agent with id {PropertyValue} does not exists");
    }
}
