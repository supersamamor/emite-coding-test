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

namespace Emite.CCM.Application.Features.CCM.Call.Commands;

public record EditCallCommand : CallState, IRequest<Validation<Error, CallState>>;

public class EditCallCommandHandler(ApplicationContext context,
                                 IMapper mapper,
                                 CompositeValidator<EditCallCommand> validator) : BaseCommandHandler<ApplicationContext, CallState, EditCallCommand>(context, mapper, validator), IRequestHandler<EditCallCommand, Validation<Error, CallState>>
{ 
    
public async Task<Validation<Error, CallState>> Handle(EditCallCommand request, CancellationToken cancellationToken) =>
		await Validators.ValidateTAsync(request, cancellationToken).BindT(
			async request => await Edit(request, cancellationToken));
	
}

public class EditCallCommandValidator : AbstractValidator<EditCallCommand>
{
    readonly ApplicationContext _context;

    public EditCallCommandValidator(ApplicationContext context)
    {
        _context = context;
		RuleFor(x => x.Id).MustAsync(async (id, cancellation) => await _context.Exists<CallState>(x => x.Id == id, cancellationToken: cancellation))
                          .WithMessage("Call with id {PropertyValue} does not exists");
        
    }
}
