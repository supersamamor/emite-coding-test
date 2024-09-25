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
using Microsoft.Extensions.Caching.Memory;
using Emite.CCM.Application.Features.CCM.Agent.Queries;
using Emite.CCM.Application.Services;

namespace Emite.CCM.Application.Features.CCM.Agent.Commands;

public record EditAgentCommand : AgentState, IRequest<Validation<Error, AgentState>>;

public class EditAgentCommandHandler(ApplicationContext context,
                                 IMapper mapper,
                                 CompositeValidator<EditAgentCommand> validator, CacheService? cacheService) : BaseCommandHandler<ApplicationContext, AgentState, EditAgentCommand>(context, mapper, validator), IRequestHandler<EditAgentCommand, Validation<Error, AgentState>>
{

    public async Task<Validation<Error, AgentState>> Handle(EditAgentCommand request, CancellationToken cancellationToken) =>
            await Validators.ValidateTAsync(request, cancellationToken).BindT(
                async request => await EditAgent(request, cancellationToken));

    protected async Task<Validation<Error, AgentState>> EditAgent(EditAgentCommand request, CancellationToken cancellationToken = default) =>
      await Context.GetSingle<AgentState>(e => e.Id == request.Id, tracking: true, cancellationToken: cancellationToken).MatchAsync(
          Some: async entity =>
          {
              Mapper.Map(request, entity);
              Context.Update(entity);
              _ = await Context.SaveChangesAsync(cancellationToken);
              if (cacheService != null)
              {
                  cacheService.RemoveCacheForQuery(nameof(GetAgentQuery));
                  cacheService.SetCache(nameof(GetAgentByIdQuery), request.Id, entity);
              }
              return Success<Error, AgentState>(entity);
          },
          None: () => Fail<Error, AgentState>($"Record with id {request.Id} does not exist"));
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
