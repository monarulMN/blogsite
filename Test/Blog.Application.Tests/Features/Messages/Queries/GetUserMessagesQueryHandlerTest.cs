using Autofac.Extras.Moq;
using BlogSite.Application.Contracts;
using BlogSite.Application.Contracts.Repositories;
using BlogSite.Application.Features.Messages.Queries;
using BlogSite.Domain;
using BlogSite.Domain.Entities;
using BlogSite.Domain.Enums;
using Moq;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Application.Tests.Features.Messages.Queries
{
    public class GetUserMessagesQueryHandlerTest
    {
        private AutoMock _mock;
        private GetUserMessagesQueryHandler _handler;
        private Mock<IApplicationUnitOfWork> _unitOfWorkMock;
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
            _messageRepositoryMock = _mock.Mock<IMessageRepository>();

            _unitOfWorkMock.SetupGet(x => x.MessageRepository)
                           .Returns(_messageRepositoryMock.Object);

            _handler = _mock.Create<GetUserMessagesQueryHandler>();
        }

        [TearDown]
        public void TearDown()
        {
            _unitOfWorkMock.Reset();
            _messageRepositoryMock.Reset();
        }

        [Test]
        public async Task Handle_ValidUserId_ReturnsUserMessages()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var expectedMessages = new List<Message>
            {
                new Message 
                { 
                    Id = Guid.NewGuid(), 
                    SenderId = userId, 
                    RecipientId = Guid.NewGuid(), 
                    Text = "Hello 1", 
                    UserType = UserType.Reader, 
                    CreatedAt = DateTime.UtcNow 
                },

                new Message 
                { 
                    Id = Guid.NewGuid(), 
                    SenderId = userId, 
                    RecipientId = Guid.NewGuid(), 
                    Text = "Hello 2", 
                    UserType = UserType.Reader, 
                    CreatedAt = DateTime.UtcNow 
                }
            };

            _messageRepositoryMock
                .Setup(r => r.GetUserMessagesAsync(userId))
                .ReturnsAsync(expectedMessages)
                .Verifiable();

            var query = new GetUserMessagesQuery
            {
                UserId = userId
            };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Count.ShouldBe(expectedMessages.Count);
            result.ShouldBe(expectedMessages);

            _messageRepositoryMock.VerifyAll();
        }
    }
}