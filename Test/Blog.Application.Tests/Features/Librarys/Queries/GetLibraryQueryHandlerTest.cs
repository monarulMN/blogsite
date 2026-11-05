using Autofac.Extras.Moq;
using BlogSite.Application.Contracts;
using BlogSite.Application.Contracts.Repositories;
using BlogSite.Application.Features.Librarys.Queries;
using BlogSite.Domain;
using BlogSite.Domain.Entities;
using Moq;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Application.Tests.Features.Librarys.Queries
{
    public class GetLibraryQueryHandlerTest
    {
        private AutoMock _mock;
        private GetLibraryQueryHandler _handler;
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

            _handler = _mock.Create<GetLibraryQueryHandler>();
        }

        [TearDown]
        public void TearDown()
        {
            _unitOfWorkMock.Reset();
            _libraryRepositoryMock.Reset();
        }

        [Test]
        public async Task Handle_ValidQuery_ReturnsUserLibraries()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var query = new GetLibraryQuery
            {
                ApplicationUserId = userId
            };

            var libraries = new List<Library>
            {
                new Library 
                { 
                    Id = Guid.NewGuid(), 
                    ApplicationUserId = userId, 
                    BlogPostId = Guid.NewGuid(), 
                    CreatedAt = DateTime.UtcNow },

                new Library 
                { 
                    Id = Guid.NewGuid(), 
                    ApplicationUserId = userId, 
                    BlogPostId = Guid.NewGuid(), 
                    CreatedAt = DateTime.UtcNow }
            };

            _libraryRepositoryMock
                .Setup(r => r.GetUserLibraryAsync(userId, CancellationToken.None))
                .ReturnsAsync(libraries)
                .Verifiable();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            this.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.ShouldBeAssignableTo<IList<Library>>(),
                () => result.Count.ShouldBe(libraries.Count)
            );
        }
    }
}