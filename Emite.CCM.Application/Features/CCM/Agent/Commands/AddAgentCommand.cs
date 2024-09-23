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

namespace Emite.CCM.Application.Features.CCM.Agent.Commands;

public record AddAgentCommand : AgentState, IRequest<Validation<Error, AgentState>>;

public class AddAgentCommandHandler(ApplicationContext context,
                                IMapper mapper,
                                CompositeValidator<AddAgentCommand> validator,
                                    IdentityContext identityContext) : BaseCommandHandler<ApplicationContext, AgentState, AddAgentCommand>(context, mapper, validator), IRequestHandler<AddAgentCommand, Validation<Error, AgentState>>
{
    
public async Task<Validation<Error, AgentState>> Handle(AddAgentCommand request, CancellationToken cancellationToken) =>
		await Validators.ValidateTAsync(request, cancellationToken).BindT(
			async request => await Add(request, cancellationToken));
	
}

public class AddAgentCommandValidator : AbstractValidator<AddAgentCommand>
{
    readonly ApplicationContext _context;

    public AddAgentCommandValidator(ApplicationContext context)
    {
        _context = context;

        RuleFor(x => x.Id).MustAsync(async (id, cancellation) => await _context.NotExists<AgentState>(x => x.Id == id, cancellationToken: cancellation))
                          .WithMessage("Agent with id {PropertyValue} already exists");
        
    }
}
