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
using Microsoft.Extensions.Caching.Memory;
using static LanguageExt.Prelude;
using Emite.CCM.Application.Features.CCM.Agent.Queries;
using Emite.CCM.Application.Services;

namespace Emite.CCM.Application.Features.CCM.Agent.Commands;

public record DeleteAgentCommand : BaseCommand, IRequest<Validation<Error, AgentState>>;

public class DeleteAgentCommandHandler(ApplicationContext context,
                                   IMapper mapper,
                                   CompositeValidator<DeleteAgentCommand> validator, CacheService? cacheService) : BaseCommandHandler<ApplicationContext, AgentState, DeleteAgentCommand>(context, mapper, validator), IRequestHandler<DeleteAgentCommand, Validation<Error, AgentState>>
{
    public async Task<Validation<Error, AgentState>> Handle(DeleteAgentCommand request, CancellationToken cancellationToken) =>
        await Validators.ValidateTAsync(request, cancellationToken).BindT(
            async request => await Delete(request.Id, cancellationToken));

    protected async Task<Validation<Error, AgentState>> DeleteAgent(string id, CancellationToken cancellationToken = default) =>
      await Context.GetSingle<AgentState>(e => e.Id == id, cancellationToken: cancellationToken).MatchAsync(
          Some: async entity =>
          {
              Context.Remove(entity);
              _ = await Context.SaveChangesAsync(cancellationToken);
              if (cacheService != null)
              {
                  cacheService.RemoveCacheForQuery(nameof(GetAgentQuery));
                  cacheService.RemoveCache(nameof(GetAgentByIdQuery), id);
              }
              return Success<Error, AgentState>(entity);
          },
          None: () => Fail<Error, AgentState>($"Record with id {id} does not exist"));
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
