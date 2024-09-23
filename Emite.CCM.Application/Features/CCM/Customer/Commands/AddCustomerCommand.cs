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

namespace Emite.CCM.Application.Features.CCM.Customer.Commands;

public record AddCustomerCommand : CustomerState, IRequest<Validation<Error, CustomerState>>;

public class AddCustomerCommandHandler(ApplicationContext context,
                                IMapper mapper,
                                CompositeValidator<AddCustomerCommand> validator,
                                    IdentityContext identityContext) : BaseCommandHandler<ApplicationContext, CustomerState, AddCustomerCommand>(context, mapper, validator), IRequestHandler<AddCustomerCommand, Validation<Error, CustomerState>>
{
    
public async Task<Validation<Error, CustomerState>> Handle(AddCustomerCommand request, CancellationToken cancellationToken) =>
		await Validators.ValidateTAsync(request, cancellationToken).BindT(
			async request => await Add(request, cancellationToken));
	
}

public class AddCustomerCommandValidator : AbstractValidator<AddCustomerCommand>
{
    readonly ApplicationContext _context;

    public AddCustomerCommandValidator(ApplicationContext context)
    {
        _context = context;

        RuleFor(x => x.Id).MustAsync(async (id, cancellation) => await _context.NotExists<CustomerState>(x => x.Id == id, cancellationToken: cancellation))
                          .WithMessage("Customer with id {PropertyValue} already exists");
        
    }
}
