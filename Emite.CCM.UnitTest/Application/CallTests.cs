using AutoMapper;
using Emite.CCM.Application.Features.CCM;
using Emite.CCM.Application.Features.CCM.Call.Commands;
using Emite.CCM.Application.Features.CCM.Call.Queries;
using Emite.CCM.Core.CCM;
using Emite.CCM.Infrastructure.Data;
using Emite.Common.Identity.Abstractions;
using Emite.Common.Utility.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Emite.CCM.UnitTest.Application
{

    [TestFixture]
    public class CallTests
    {
        private ApplicationContext _context;
        private IMapper _mapper;
        private IdentityContext _identityContext;
        private string callId = "1";
        [SetUp]
        public void Setup()
        {
            // Set up the in-memory database
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryCallDB")
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
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test, Order(1)]
        public async Task Handle_Should_Add_Call_When_Valid()
        {
            var command = new AddCallCommand
            {
                Id = callId,
                Notes = "test notes",
                StartTime = DateTime.Now,
            };
            var validators = new List<IValidator<AddCallCommand>>
            {
                new AddCallCommandValidator(_context)
            };
            var validator = new CompositeValidator<AddCallCommand>(validators);
            var handler = new AddCallCommandHandler(_context, _mapper, validator, _identityContext);
            var result = await handler.Handle(command, CancellationToken.None);
            // Assert
            Assert.IsTrue(result.IsSuccess);
            var call = await _context.Call.FirstOrDefaultAsync(c => c.Id == callId);
            Assert.IsNotNull(call);
            Assert.That(call.Id, Is.EqualTo(callId!));
        }
        [Test, Order(2)]
        public async Task Handle_Should_Update_Call_Record()
        {
            string customerId = "2";
            var command = new EditCallCommand
            {
                Id = callId,
                CustomerId = customerId
            };
            var validators = new List<IValidator<EditCallCommand>>
            {
                new EditCallCommandValidator(_context)
            };
            var validator = new CompositeValidator<EditCallCommand>(validators);
            var handler = new EditCallCommandHandler(_context, _mapper, validator);
            var result = await handler.Handle(command, CancellationToken.None);
            // Assert
            Assert.IsTrue(result.IsSuccess);
            var call = await _context.Call.FirstOrDefaultAsync(c => c.Id == callId);
            Assert.IsNotNull(call);
            Assert.That(call.CustomerId, Is.EqualTo(customerId));
        }
        [Test, Order(3)]
        public async Task Handle_Should_Fetch_Call_Record()
        {           
            var query = new GetCallByIdQuery(callId);
            var handler = new GetCallByIdQueryHandler(_context);
            var result = await handler.Handle(query, CancellationToken.None);
            CallState? call = null;
            _ = result.Select(l => call = l);
            // Assert       
            Assert.IsNotNull(call);
        }
        [Test, Order(4)]
        public async Task Handle_Should_Delete_Call()
        {
            var command = new DeleteCallCommand
            {
                Id = callId,
            };
            var validators = new List<IValidator<DeleteCallCommand>>
            {
                new DeleteCallCommandValidator(_context)
            };
            var validator = new CompositeValidator<DeleteCallCommand>(validators);
            var handler = new DeleteCallCommandHandler(_context, _mapper, validator);
            var result = await handler.Handle(command, CancellationToken.None);
            // Assert
            Assert.IsTrue(result.IsSuccess);
            var call = await _context.Call.FirstOrDefaultAsync(c => c.Id == callId);
            Assert.IsNull(call);
        }
    }
}
