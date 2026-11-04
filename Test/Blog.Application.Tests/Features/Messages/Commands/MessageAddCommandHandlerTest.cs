using Autofac.Extras.Moq;
using BlogSite.Application.Contracts;
using BlogSite.Application.Contracts.Repositories;
using BlogSite.Application.Features.Messages.Commands;
using BlogSite.Domain;
using BlogSite.Domain.Entities;
using BlogSite.Domain.Enums;
using MapsterMapper;
using Moq;
using NUnit.Framework;
using Shouldly;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DevSkill.Application.Tests
{
    public class MessageAddCommandHandlerTest
    {
        private AutoMock _mock;
        private MessageAddCommandHandler _handler;
        private Mock<IApplicationUnitOfWork> _unitOfWorkMock;
        private Mock<IMapper> _mapperMock;
        private Mock<IMessageRepository> _messageRepositoryMock;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _mock = AutoMock.GetLoose();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _mock.Dispose();
        }

        [SetUp]
        public void Setup()
        {
            _unitOfWorkMock = _mock.Mock<IApplicationUnitOfWork>();
            _mapperMock = _mock.Mock<IMapper>();
            _messageRepositoryMock = _mock.Mock<IMessageRepository>();

            _unitOfWorkMock.SetupGet(x => x.MessageRepository)
                           .Returns(_messageRepositoryMock.Object);

            _handler = _mock.Create<MessageAddCommandHandler>();
        }

        [TearDown]
        public void TearDown()
        {
            _unitOfWorkMock.Reset();
            _mapperMock.Reset();
            _messageRepositoryMock.Reset();
        }

        [Test]
        public async Task Handle_ValidCommand_AddsMessageSuccessfully()
        {
            // Arrange
            var command = new MessageAddCommand
            {
                SenderId = Guid.NewGuid(),
                RecipientId = Guid.NewGuid(),
                UserType = UserType.Reader,
                Text = "Hello, this is a test message.",
                CreatedAt = DateTime.UtcNow
            };

            var mappedMessage = new Message
            {
                SenderId = command.SenderId,
                RecipientId = command.RecipientId,
                UserType = command.UserType,
                Text = command.Text,
                CreatedAt = command.CreatedAt
            };

            _mapperMock.Setup(m => m.Map<Message>(command))
                       .Returns(mappedMessage)
                       .Verifiable();

            _messageRepositoryMock.Setup(r => r.AddAsync(mappedMessage, CancellationToken.None))
                                  .Returns(Task.CompletedTask)
                                  .Verifiable();

            _unitOfWorkMock.Setup(u => u.SaveAsync(CancellationToken.None))
                           .Returns(Task.CompletedTask)
                           .Verifiable();

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            this.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.ShouldBeOfType<Message>(),
                () => result.Id.ShouldNotBe(Guid.Empty),
                () => result.Text.ShouldBe(command.Text),
                () => result.SenderId.ShouldBe(command.SenderId),
                () => result.RecipientId.ShouldBe(command.RecipientId),
                () => result.UserType.ShouldBe(command.UserType)
            );
        }
    }
}