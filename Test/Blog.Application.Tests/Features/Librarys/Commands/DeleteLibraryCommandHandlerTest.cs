using Autofac.Extras.Moq;
using BlogSite.Application.Contracts;
using BlogSite.Application.Contracts.Repositories;
using BlogSite.Application.Features.Librarys.Commands;
using BlogSite.Domain;
using BlogSite.Domain.Entities;
using Moq;
using NUnit.Framework;
using Shouldly;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Application.Tests.Features.Librarys.Commands
{
    public class DeleteLibraryCommandHandlerTest
    {
        private AutoMock _mock;
        private DeleteLibraryCommandHandler _handler;
        private Mock<IApplicationUnitOfWork> _unitOfWorkMock;
        private Mock<ILibraryRepository> _libraryRepositoryMock;

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
            _libraryRepositoryMock = _mock.Mock<ILibraryRepository>();

            _unitOfWorkMock.SetupGet(x => x.LibraryRepository)
                           .Returns(_libraryRepositoryMock.Object);

            _handler = _mock.Create<DeleteLibraryCommandHandler>();
        }

        [TearDown]
        public void TearDown()
        {
            _unitOfWorkMock.Reset();
            _libraryRepositoryMock.Reset();
        }

        [Test]
        public async Task Handle_ValidCommand_DeletesLibrary()
        {
            // Arrange
            var applicationUserId = Guid.NewGuid();
            var blogPostId = Guid.NewGuid();
            var command = new DeleteLibraryCommand
            {
                ApplicationUserId = applicationUserId,
                BlogPostId = blogPostId
            };

            var library = new Library
            {
                Id = Guid.NewGuid(),
                ApplicationUserId = applicationUserId,
                BlogPostId = blogPostId,
                CreatedAt = DateTime.UtcNow
            };

            // Mock GetLibraryAsync
            _libraryRepositoryMock
                .Setup(r => r.GetLibraryAsync(applicationUserId, blogPostId, CancellationToken.None))
                .ReturnsAsync(library)
                .Verifiable();

            // Mock RemoveAsync
            _libraryRepositoryMock
                .Setup(r => r.RemoveAsync(library, CancellationToken.None))
                .Returns(Task.CompletedTask)
                .Verifiable();

            // Mock SaveAsync
            _unitOfWorkMock
                .Setup(u => u.SaveAsync(CancellationToken.None))
                .Returns(Task.CompletedTask)
                .Verifiable();

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            this.ShouldSatisfyAllConditions(
                () => result.ShouldBe(library.Id),
                () => _libraryRepositoryMock.VerifyAll(),
                () => _unitOfWorkMock.VerifyAll()
            );
        }
    }
}