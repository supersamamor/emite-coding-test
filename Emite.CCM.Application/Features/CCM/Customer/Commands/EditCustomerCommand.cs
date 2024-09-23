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

public record EditCustomerCommand : CustomerState, IRequest<Validation<Error, CustomerState>>;

public class EditCustomerCommandHandler(ApplicationContext context,
                                 IMapper mapper,
                                 CompositeValidator<EditCustomerCommand> validator) : BaseCommandHandler<ApplicationContext, CustomerState, EditCustomerCommand>(context, mapper, validator), IRequestHandler<EditCustomerCommand, Validation<Error, CustomerState>>
{ 
    
public async Task<Validation<Error, CustomerState>> Handle(EditCustomerCommand request, CancellationToken cancellationToken) =>
		await Validators.ValidateTAsync(request, cancellationToken).BindT(
			async request => await Edit(request, cancellationToken));
	
}

public class EditCustomerCommandValidator : AbstractValidator<EditCustomerCommand>
{
    readonly ApplicationContext _context;

    public EditCustomerCommandValidator(ApplicationContext context)
    {
        _context = context;
		RuleFor(x => x.Id).MustAsync(async (id, cancellation) => await _context.Exists<CustomerState>(x => x.Id == id, cancellationToken: cancellation))
                          .WithMessage("Customer with id {PropertyValue} does not exists");
        
    }
}
