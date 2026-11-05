using Autofac.Extras.Moq;
using BlogSite.Application.Contracts;
using BlogSite.Application.Contracts.Repositories;
using BlogSite.Application.Features.Comments.Commands;
using BlogSite.Domain;
using BlogSite.Domain.Entities;
using Moq;
using NUnit.Framework;
using Shouldly;
using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Application.Tests.Features.Comments.Commands
{
    public class CommentDeleteCommandHandlerTest
    {
        private AutoMock _mock;
        private CommentDeleteCommandHandler _handler;
        private Mock<IApplicationUnitOfWork> _unitOfWorkMock;
        private Mock<ICommentRepository> _commentRepositoryMock;

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
            _commentRepositoryMock = _mock.Mock<ICommentRepository>();

            // Wire up the repository
            _unitOfWorkMock.SetupGet(x => x.CommentRepository)
                .Returns(_commentRepositoryMock.Object);

            _handler = _mock.Create<CommentDeleteCommandHandler>();
        }

        [TearDown]
        public void TearDown()
        {
            _unitOfWorkMock.Reset();
            _commentRepositoryMock.Reset();
        }

        [Test]
        public async Task Handle_ValidCommand_DeletesComment()
        {
            // Arrange
            var commentId = Guid.NewGuid();
            var command = new CommentDeleteCommand
            {
                CommentId = commentId
            };

            // Setup RemoveAsync
            _commentRepositoryMock
                .Setup(r => r.RemoveAsync(It.IsAny<Expression<Func<Comment, bool>>>(), CancellationToken.None))
                .Returns(Task.CompletedTask)
                .Verifiable();

            // Setup SaveAsync
            _unitOfWorkMock
                .Setup(u => u.SaveAsync(CancellationToken.None))
                .Returns(Task.CompletedTask)
                .Verifiable();

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            this.ShouldSatisfyAllConditions(
                () => result.ShouldBe(commentId),
                () => _commentRepositoryMock.VerifyAll(),
                () => _unitOfWorkMock.VerifyAll()
            );
        }
    }
}