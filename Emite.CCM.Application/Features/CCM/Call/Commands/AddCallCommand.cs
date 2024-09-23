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

public record AddCallCommand : CallState, IRequest<Validation<Error, CallState>>;

public class AddCallCommandHandler(ApplicationContext context,
                                IMapper mapper,
                                CompositeValidator<AddCallCommand> validator,
                                    IdentityContext identityContext) : BaseCommandHandler<ApplicationContext, CallState, AddCallCommand>(context, mapper, validator), IRequestHandler<AddCallCommand, Validation<Error, CallState>>
{
    
public async Task<Validation<Error, CallState>> Handle(AddCallCommand request, CancellationToken cancellationToken) =>
		await Validators.ValidateTAsync(request, cancellationToken).BindT(
			async request => await Add(request, cancellationToken));
	
}

public class AddCallCommandValidator : AbstractValidator<AddCallCommand>
{
    readonly ApplicationContext _context;

    public AddCallCommandValidator(ApplicationContext context)
    {
        _context = context;

        RuleFor(x => x.Id).MustAsync(async (id, cancellation) => await _context.NotExists<CallState>(x => x.Id == id, cancellationToken: cancellation))
                          .WithMessage("Call with id {PropertyValue} already exists");
        
    }
}
