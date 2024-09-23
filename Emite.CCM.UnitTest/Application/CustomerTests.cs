using AutoMapper;
using Emite.CCM.Application.Features.CCM;
using Emite.CCM.Application.Features.CCM.Customer.Commands;
using Emite.CCM.Application.Features.CCM.Customer.Queries;
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
    public class CustomerTests
    {
        private ApplicationContext _context;
        private IMapper _mapper;
        private IdentityContext _identityContext;
        private string customerId = "1";
        [SetUp]
        public void Setup()
        {
            // Set up the in-memory database
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryCustomerDB")
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
        public async Task Handle_Should_Add_Customer_When_Valid()
        {
            var command = new AddCustomerCommand
            {
                Id = customerId,
            };
            var validators = new List<IValidator<AddCustomerCommand>>
            {
                new AddCustomerCommandValidator(_context)
            };
            var validator = new CompositeValidator<AddCustomerCommand>(validators);
            var handler = new AddCustomerCommandHandler(_context, _mapper, validator, _identityContext);
            var result = await handler.Handle(command, CancellationToken.None);
            // Assert
            Assert.IsTrue(result.IsSuccess);
            var customer = await _context.Customer.FirstOrDefaultAsync(c => c.Id == customerId);
            Assert.IsNotNull(customer);
            Assert.That(customer.Id, Is.EqualTo(customerId!));
        }
        [Test, Order(2)]
        public async Task Handle_Should_Update_Customer_Record()
        {
            string name = "Test";
            var command = new EditCustomerCommand
            {
                Id = customerId,
                Name = name
            };
            var validators = new List<IValidator<EditCustomerCommand>>
            {
                new EditCustomerCommandValidator(_context)
            };
            var validator = new CompositeValidator<EditCustomerCommand>(validators);
            var handler = new EditCustomerCommandHandler(_context, _mapper, validator);
            var result = await handler.Handle(command, CancellationToken.None);
            // Assert
            Assert.IsTrue(result.IsSuccess);
            var customer = await _context.Customer.FirstOrDefaultAsync(c => c.Id == customerId);
            Assert.IsNotNull(customer);
            Assert.That(customer.Name, Is.EqualTo(name));
        }
        [Test, Order(3)]
        public async Task Handle_Should_Fetch_Customer_Record()
        {
            var query = new GetCustomerByIdQuery(customerId);
            var handler = new GetCustomerByIdQueryHandler(_context);
            var result = await handler.Handle(query, CancellationToken.None);
            CustomerState? customer = null;
            _ = result.Select(l => customer = l);
            // Assert       
            Assert.IsNotNull(customer);
        }
        [Test, Order(4)]
        public async Task Handle_Should_Delete_Customer()
        {
            var command = new DeleteCustomerCommand
            {
                Id = customerId,
            };
            var validators = new List<IValidator<DeleteCustomerCommand>>
            {
                new DeleteCustomerCommandValidator(_context)
            };
            var validator = new CompositeValidator<DeleteCustomerCommand>(validators);
            var handler = new DeleteCustomerCommandHandler(_context, _mapper, validator);
            var result = await handler.Handle(command, CancellationToken.None);
            // Assert
            Assert.IsTrue(result.IsSuccess);
            var customer = await _context.Customer.FirstOrDefaultAsync(c => c.Id == customerId);
            Assert.IsNull(customer);
        }
    }
}
