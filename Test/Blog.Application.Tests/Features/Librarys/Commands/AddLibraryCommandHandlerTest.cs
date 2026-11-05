using Autofac.Extras.Moq;
using BlogSite.Application.Contracts;
using BlogSite.Application.Contracts.Repositories;
using BlogSite.Application.Features.Librarys.Commands;
using BlogSite.Domain;
using BlogSite.Domain.Entities;
using MapsterMapper;
using Moq;
using NUnit.Framework;
using Shouldly;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Application.Tests.Features.Librarys.Commands
{
    public class AddLibraryCommandHandlerTest
    {
        private AutoMock _mock;
        private AddLibraryCommandHandler _handler;
        private Mock<IApplicationUnitOfWork> _unitOfWorkMock;
        private Mock<ILibraryRepository> _libraryRepositoryMock;
        private Mock<IMapper> _mapperMock;

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
            _mapperMock = _mock.Mock<IMapper>();

            _unitOfWorkMock.SetupGet(x => x.LibraryRepository)
                           .Returns(_libraryRepositoryMock.Object);

            _handler = _mock.Create<AddLibraryCommandHandler>();
        }

        [TearDown]
        public void TearDown()
        {
            _unitOfWorkMock.Reset();
            _libraryRepositoryMock.Reset();
            _mapperMock.Reset();
        }

        [Test]
        public async Task Handle_ValidCommand_AddsLibrary()
        {
            // Arrange
            var command = new AddLibraryCommand
            {
                ApplicationUserId = Guid.NewGuid(),
                BlogPostId = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow
            };

            var mappedLibrary = new Library
            {
                ApplicationUserId = command.ApplicationUserId,
                BlogPostId = command.BlogPostId,
                CreatedAt = command.CreatedAt
            };

            // Mock Mapper.Map
            _mapperMock
                .Setup(m => m.Map<Library>(command))
                .Returns(mappedLibrary)
                .Verifiable();

            // Mock AddAsync
            _libraryRepositoryMock
                .Setup(r => r.AddAsync(mappedLibrary, CancellationToken.None))
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
                () => result.ShouldNotBeNull(),
                () => result.ShouldBeOfType<Library>(),
                () => result.Id.ShouldNotBe(Guid.Empty),
                () => result.ApplicationUserId.ShouldBe(command.ApplicationUserId),
                () => result.BlogPostId.ShouldBe(command.BlogPostId)
            );
        }
    }
}