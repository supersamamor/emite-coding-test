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

namespace Emite.CCM.Application.Features.CCM.Customer.Commands;

public record DeleteCustomerCommand : BaseCommand, IRequest<Validation<Error, CustomerState>>;

public class DeleteCustomerCommandHandler(ApplicationContext context,
                                   IMapper mapper,
                                   CompositeValidator<DeleteCustomerCommand> validator) : BaseCommandHandler<ApplicationContext, CustomerState, DeleteCustomerCommand>(context, mapper, validator), IRequestHandler<DeleteCustomerCommand, Validation<Error, CustomerState>>
{ 
    public async Task<Validation<Error, CustomerState>> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken) =>
        await Validators.ValidateTAsync(request, cancellationToken).BindT(
            async request => await Delete(request.Id, cancellationToken));
}


public class DeleteCustomerCommandValidator : AbstractValidator<DeleteCustomerCommand>
{
    readonly ApplicationContext _context;

    public DeleteCustomerCommandValidator(ApplicationContext context)
    {
        _context = context;
        RuleFor(x => x.Id).MustAsync(async (id, cancellation) => await _context.Exists<CustomerState>(x => x.Id == id, cancellationToken: cancellation))
                          .WithMessage("Customer with id {PropertyValue} does not exists");
    }
}
