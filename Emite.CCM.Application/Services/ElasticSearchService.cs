using Emite.CCM.Core.CCM;
using LanguageExt;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Nest;
using System.Net.Sockets;
namespace Emite.CCM.Application.Services
{
    public class ElasticSearchService
    {
        private readonly IElasticClient? _elasticClient;
        private readonly ILogger<ElasticSearchService> _logger;
        public ElasticSearchService(IConfiguration configuration, ILogger<ElasticSearchService> logger)
        {
            var uriString = configuration["Elasticsearch:Uri"];
            if (!string.IsNullOrEmpty(uriString))
            {
                var settings = new ConnectionSettings(new Uri(uriString))
                .DefaultIndex("tickets");
                _elasticClient = new ElasticClient(settings);
            }
            else
            {
                _elasticClient = null;
            }
            _logger = logger;
        }

        public async Task IndexTicketAsync(TicketState ticket)
        {
            if (_elasticClient == null) { return; }
            var response = await _elasticClient.IndexDocumentAsync(ticket);
            if (!response.IsValid)
            {
                throw new Exception($"Failed to index ticket: {response.ServerError}");
            }
        }

        public async Task UpdateTicketAsync(TicketState ticket)
        {
            if (_elasticClient == null) { return; }
            var response = await _elasticClient.UpdateAsync<TicketState>(ticket.Id, u => u
                .Doc(ticket)
            );
            if (!response.IsValid)
            {
                throw new Exception($"Failed to update ticket: {response.ServerError}");
            }
        }

        public async Task DeleteTicketAsync(string ticketId)
        {
            if (_elasticClient == null) { return; }
            var response = await _elasticClient.DeleteAsync<TicketState>(ticketId);
            if (!response.IsValid)
            {
                throw new Exception($"Failed to delete ticket: {response.ServerError}");
            }
        }
        public async Task CreateIndexIfNotExistsAsync()
        {
            if (_elasticClient == null) { return; }
            var indexName = "tickets";
            var existsResponse = await _elasticClient.Indices.ExistsAsync(indexName);
            if (!existsResponse.Exists)
            {
                var createIndexResponse = await _elasticClient.Indices.CreateAsync(indexName, c => c
                    .Settings(s => s
                        .NumberOfShards(1)
                        .NumberOfReplicas(1)
                        .Analysis(a => a
                            .Analyzers(ad => ad
                                .Custom("custom_analyzer", ca => ca
                                    .Tokenizer("standard")
                                    .Filters("lowercase", "asciifolding")
                                )
                            )
                        )
                    )
                    .Map<TicketState>(m => m
                        .AutoMap()
                        .Properties(ps => ps
                            .Keyword(k => k
                                .Name(n => n.AgentId)
                            )
                            .Date(d => d
                                .Name(n => n.CreatedAt)
                                .Format("strict_date_optional_time||epoch_millis")
                            )
                            .Keyword(k => k
                                .Name(n => n.CustomerId)
                            )
                            .Text(t => t
                                .Name(n => n.Description)
                                .Analyzer("custom_analyzer")
                            )
                            .Keyword(k => k
                                .Name(n => n.Priority)
                            )
                            .Text(t => t
                                .Name(n => n.Resolution)
                                .Analyzer("custom_analyzer")
                            )
                            .Keyword(k => k
                                .Name(n => n.Status)
                            )
                            .Date(d => d
                                .Name(n => n.UpdatedAt)
                                .Format("strict_date_optional_time||epoch_millis")
                            )
                            .Nested<AgentState>(n => n
                                .Name(t => t.Agent)
                                .Properties(ap => ap
                                    .Keyword(k => k
                                        .Name(a => a.Email)
                                    )
                                    .Text(tn => tn
                                        .Name(a => a.Name)
                                        .Analyzer("custom_analyzer")
                                    )
                                    .Keyword(k => k
                                        .Name(a => a.PhoneExtension)
                                    )
                                    .Keyword(k => k
                                        .Name(a => a.Status)
                                    )
                                )
                            )
                            .Nested<CustomerState>(n => n
                                .Name(t => t.Customer)
                                .Properties(cp => cp
                                    .Keyword(k => k
                                        .Name(c => c.Id)
                                    )
                                    .Text(ct => ct
                                        .Name(c => c.Name)
                                        .Analyzer("custom_analyzer")
                                    )
                                    .Keyword(k => k
                                        .Name(c => c.Email)
                                    )
                                // Add additional customer properties
                                )
                            )
                        )
                    )
                );
                if (!createIndexResponse.IsValid)
                {
                    _logger.LogError($"Failed to create index: {createIndexResponse.ServerError}");
                }
            }
        }

    }
}
