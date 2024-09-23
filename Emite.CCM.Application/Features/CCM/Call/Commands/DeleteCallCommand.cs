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

namespace Emite.CCM.Application.Features.CCM.Call.Commands;

public record DeleteCallCommand : BaseCommand, IRequest<Validation<Error, CallState>>;

public class DeleteCallCommandHandler(ApplicationContext context,
                                   IMapper mapper,
                                   CompositeValidator<DeleteCallCommand> validator) : BaseCommandHandler<ApplicationContext, CallState, DeleteCallCommand>(context, mapper, validator), IRequestHandler<DeleteCallCommand, Validation<Error, CallState>>
{ 
    public async Task<Validation<Error, CallState>> Handle(DeleteCallCommand request, CancellationToken cancellationToken) =>
        await Validators.ValidateTAsync(request, cancellationToken).BindT(
            async request => await Delete(request.Id, cancellationToken));
}


public class DeleteCallCommandValidator : AbstractValidator<DeleteCallCommand>
{
    readonly ApplicationContext _context;

    public DeleteCallCommandValidator(ApplicationContext context)
    {
        _context = context;
        RuleFor(x => x.Id).MustAsync(async (id, cancellation) => await _context.Exists<CallState>(x => x.Id == id, cancellationToken: cancellation))
                          .WithMessage("Call with id {PropertyValue} does not exists");
    }
}
