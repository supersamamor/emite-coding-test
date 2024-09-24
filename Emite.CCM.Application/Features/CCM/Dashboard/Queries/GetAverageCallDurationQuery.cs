using Emite.CCM.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using LanguageExt;

namespace Emite.CCM.Application.Features.CCM.AverageCallDuration.Queries;

public record GetAverageCallDurationQuery : IRequest<double?>
{
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
    public string? Status { get; set; }
    public string? AgentId { get; set; }
}

public class GetAverageCallDurationQueryHandler(ApplicationContext context) : IRequest
{
    public async Task<double?> Handle(GetAverageCallDurationQuery request, CancellationToken cancellationToken = default)
    {
        // Initialize the query with necessary includes
        var query = context.Call
                           .AsNoTracking()
                           .Include(l => l.Agent)
                           .Include(l => l.Customer).IgnoreQueryFilters();

        // Apply filters based on the request
        if (request.DateFrom != null && request.DateFrom != DateTime.MinValue)
        {
            query = query.Where(l => l.StartTime >= request.DateFrom);
        }
        if (request.DateTo != null && request.DateTo != DateTime.MinValue)
        {
            query = query.Where(l => l.EndTime <= request.DateTo);
        }
        if (!string.IsNullOrEmpty(request.Status))
        {
            query = query.Where(l => l.Status == request.Status);
        }
        if (!string.IsNullOrEmpty(request.AgentId))
        {
            query = query.Where(l => l.AgentId == request.AgentId);
        }
        var durationsQuery = query.Select(e =>
            EF.Functions.DateDiffSecond(e.StartTime, e.EndTime)
        );
        var averageDurationInSeconds = await durationsQuery.AverageAsync(cancellationToken);
        return averageDurationInSeconds;
    }
}
