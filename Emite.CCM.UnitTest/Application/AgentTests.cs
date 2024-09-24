using AutoMapper;
using Emite.CCM.Application;
using Emite.CCM.Application.Features.CCM;
using Emite.CCM.Application.Features.CCM.Agent.Commands;
using Emite.CCM.Application.Features.CCM.Agent.Queries;
using Emite.CCM.Core.CCM;
using Emite.CCM.Infrastructure.Data;
using Emite.Common.Identity.Abstractions;
using Emite.Common.Utility.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework.Legacy;


namespace Emite.CCM.UnitTest
{

    [TestFixture]
    public class AgentTests : TestBase
    {
        private ApplicationContext _context;
        private IMapper _mapper;
        private IdentityContext _identityContext;
        private string agentId = "1";
        private IMemoryCache _memoryCache;
        private IOptions<CacheSettings> _cacheSettings;
        [SetUp]
        public void Setup()
        {
            // Set up the in-memory database
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryAgentDB")
                .Options;
            _context = new ApplicationContext(options, new Mock<IAuthenticatedUser>().Object);
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CCMProfile>();
            });
            _mapper = config.CreateMapper();
            var optionsIdentity = new DbContextOptionsBuilder<IdentityContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryIdentityDB")
                .Options;
            _identityContext = new IdentityContext(optionsIdentity);
            _memoryCache = new MemoryCache(new MemoryCacheOptions());

            // Retrieve IOptions<CacheSettings>
            _cacheSettings = ServiceProvider.GetService<IOptions<CacheSettings>>();
            if (options == null)
            {
                throw new InvalidOperationException("CacheSettings are not configured.");
            }
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
            _memoryCache.Dispose();
        }
        [TearDown]
        public void TearDown()
        {
            // Check the environment variable
            var performTeardown = Environment.GetEnvironmentVariable("PERFORM_TEARDOWN");

            if (!string.Equals(performTeardown, "true", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("TearDown: Skipping teardown as per SKIP_TEARDOWN=true.");
                return;
            }
            _context.Database.EnsureDeleted();
            _context.Dispose();
            _identityContext.Database.EnsureDeleted();
            _identityContext.Dispose();
            _memoryCache.Dispose();
        }

        [Test, Order(1)]
        public async Task Handle_Should_Add_Agent_When_Valid()
        {
            var command = new AddAgentCommand
            {
                Id = agentId,
            };
            var validators = new List<IValidator<AddAgentCommand>>
            {
                new AddAgentCommandValidator(_context)
            };
            var validator = new CompositeValidator<AddAgentCommand>(validators);
            var handler = new AddAgentCommandHandler(_context, _mapper, validator, _identityContext);
            var result = await handler.Handle(command, CancellationToken.None);
            // Assert
            ClassicAssert.IsTrue(result.IsSuccess);
            var agent = await _context.Agent.FirstOrDefaultAsync(c => c.Id == agentId);
            ClassicAssert.IsNotNull(agent);
            Assert.That(agent.Id, Is.EqualTo(agentId!));
        }
        [Test, Order(2)]
        public async Task Handle_Should_Update_Agent_Record()
        {
            string name = "Test";
            var command = new EditAgentCommand
            {
                Id = agentId,
                Name = name
            };
            var validators = new List<IValidator<EditAgentCommand>>
            {
                new EditAgentCommandValidator(_context)
            };
            var validator = new CompositeValidator<EditAgentCommand>(validators);
            var handler = new EditAgentCommandHandler(_context, _mapper, validator);
            var result = await handler.Handle(command, CancellationToken.None);
            // Assert
            ClassicAssert.IsTrue(result.IsSuccess);
            var agent = await _context.Agent.FirstOrDefaultAsync(c => c.Id == agentId);
            ClassicAssert.IsNotNull(agent);
            Assert.That(agent.Name, Is.EqualTo(name));
        }
        [Test, Order(3)]
        public async Task Handle_Should_Fetch_Agent_Record()
        {
            var query = new GetAgentByIdQuery(agentId);
            var handler = new GetAgentByIdQueryHandler(_context, _memoryCache, _cacheSettings);
            var result = await handler.Handle(query, CancellationToken.None);
            AgentState? agent = null;
            _ = result.Select(l => agent = l);
            // Assert       
            ClassicAssert.IsNotNull(agent);
        }
        [Test, Order(4)]
        public async Task Handle_Should_Delete_Agent()
        {
            var command = new DeleteAgentCommand
            {
                Id = agentId,
            };
            var validators = new List<IValidator<DeleteAgentCommand>>
            {
                new DeleteAgentCommandValidator(_context)
            };
            var validator = new CompositeValidator<DeleteAgentCommand>(validators);
            var handler = new DeleteAgentCommandHandler(_context, _mapper, validator);
            var result = await handler.Handle(command, CancellationToken.None);
            // Assert
            ClassicAssert.IsTrue(result.IsSuccess);
            var customer = await _context.Agent.FirstOrDefaultAsync(c => c.Id == agentId);
            ClassicAssert.IsNull(customer);
        }
    }
}
