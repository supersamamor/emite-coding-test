using AutoMapper;
using Emite.CCM.Application.Features.CCM;
using Emite.CCM.Application.Features.CCM.Ticket.Commands;
using Emite.CCM.Application.Features.CCM.Ticket.Queries;
using Emite.CCM.Application.Hubs;
using Emite.CCM.Core.CCM;
using Emite.CCM.Infrastructure.Data;
using Emite.Common.Identity.Abstractions;
using Emite.Common.Utility.Validators;
using FluentValidation;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework.Legacy;

namespace Emite.CCM.UnitTest.Application
{

    [TestFixture]
    public class TicketTests
    {
        private ApplicationContext _context;
        private IMapper _mapper;
        private IdentityContext _identityContext;
        private string ticketId = "1";
        [SetUp]
        public void Setup()
        {
            // Set up the in-memory database
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryTicketDB")
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
        }

        [Test, Order(1)]
        public async Task Handle_Should_Add_Ticket_When_Valid()
        {
            var command = new AddTicketCommand
            {
                Id = ticketId,
            };
            var validators = new List<IValidator<AddTicketCommand>>
            {
                new AddTicketCommandValidator(_context)
            };
            var validator = new CompositeValidator<AddTicketCommand>(validators);
            var handler = new AddTicketCommandHandler(_context, _mapper, validator, _identityContext,
               null, null);
            var result = await handler.Handle(command, CancellationToken.None);
            // Assert
            ClassicAssert.IsTrue(result.IsSuccess);
            var ticket = await _context.Ticket.FirstOrDefaultAsync(c => c.Id == ticketId);
            ClassicAssert.IsNotNull(ticket);
            Assert.That(ticket.Id, Is.EqualTo(ticketId!));
        }
        [Test, Order(2)]
        public async Task Handle_Should_Update_Ticket_Record()
        {
            string decription = "Test";
            var command = new EditTicketCommand
            {
                Id = ticketId,
                Description = decription
            };
            var validators = new List<IValidator<EditTicketCommand>>
            {
                new EditTicketCommandValidator(_context)
            };
            var validator = new CompositeValidator<EditTicketCommand>(validators);
            var handler = new EditTicketCommandHandler(_context, _mapper, validator, null);
            var result = await handler.Handle(command, CancellationToken.None);
            // Assert
            ClassicAssert.IsTrue(result.IsSuccess);
            var ticket = await _context.Ticket.FirstOrDefaultAsync(c => c.Id == ticketId);
            ClassicAssert.IsNotNull(ticket);
            Assert.That(ticket.Description, Is.EqualTo(decription));
        }
        [Test, Order(3)]
        public async Task Handle_Should_Fetch_Ticket_Record()
        {
            var query = new GetTicketByIdQuery(ticketId);
            var handler = new GetTicketByIdQueryHandler(_context);
            var result = await handler.Handle(query, CancellationToken.None);
            TicketState? ticket = null;
            _ = result.Select(l => ticket = l);
            // Assert       
            ClassicAssert.IsNotNull(ticket);
        }
        [Test, Order(4)]
        public async Task Handle_Should_Delete_Ticket()
        {
            var command = new DeleteTicketCommand
            {
                Id = ticketId,
            };
            var validators = new List<IValidator<DeleteTicketCommand>>
            {
                new DeleteTicketCommandValidator(_context)
            };
            var validator = new CompositeValidator<DeleteTicketCommand>(validators);
            var handler = new DeleteTicketCommandHandler(_context, _mapper, validator, null);
            var result = await handler.Handle(command, CancellationToken.None);
            // Assert
            ClassicAssert.IsTrue(result.IsSuccess);
            var ticket = await _context.Ticket.FirstOrDefaultAsync(c => c.Id == ticketId);
            ClassicAssert.IsNull(ticket);
        }
    }
}
