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

public record EditAgentCommand : AgentState, IRequest<Validation<Error, AgentState>>;

public class EditAgentCommandHandler(ApplicationContext context,
                                 IMapper mapper,
                                 CompositeValidator<EditAgentCommand> validator) : BaseCommandHandler<ApplicationContext, AgentState, EditAgentCommand>(context, mapper, validator), IRequestHandler<EditAgentCommand, Validation<Error, AgentState>>
{ 
    
public async Task<Validation<Error, AgentState>> Handle(EditAgentCommand request, CancellationToken cancellationToken) =>
		await Validators.ValidateTAsync(request, cancellationToken).BindT(
			async request => await Edit(request, cancellationToken));
	
}

public class EditAgentCommandValidator : AbstractValidator<EditAgentCommand>
{
    readonly ApplicationContext _context;

    public EditAgentCommandValidator(ApplicationContext context)
    {
        _context = context;
		RuleFor(x => x.Id).MustAsync(async (id, cancellation) => await _context.Exists<AgentState>(x => x.Id == id, cancellationToken: cancellation))
                          .WithMessage("Agent with id {PropertyValue} does not exists");
        
    }
}
