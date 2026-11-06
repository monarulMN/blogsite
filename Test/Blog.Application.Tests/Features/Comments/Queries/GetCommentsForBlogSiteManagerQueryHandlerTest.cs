using Autofac.Extras.Moq;
using BlogSite.Application.Contracts;
using BlogSite.Application.Contracts.Repositories;
using BlogSite.Application.Features.Comments.Queries;
using BlogSite.Domain;
using BlogSite.Domain.Entities;
using Moq;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Application.Tests.Features.Comments.Queries
{
    public class GetCommentsForBlogSiteManagerQueryHandlerTest
    {
        private AutoMock _mock;
        private GetCommentsForBlogSiteManagerQueryHandler _handler;
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

            // Setup the repository property
            _unitOfWorkMock.SetupGet(x => x.CommentRepository)
                           .Returns(_commentRepositoryMock.Object);

            _handler = _mock.Create<GetCommentsForBlogSiteManagerQueryHandler>();
        }

        [TearDown]
        public void TearDown()
        {
            _unitOfWorkMock.Reset();
            _commentRepositoryMock.Reset();
        }

        [Test]
        public async Task Handle_ValidQuery_ReturnsPagedComments()
        {
            // Arrange
            var blogSiteId = Guid.NewGuid();
            var query = new GetCommentsForBlogSiteManagerQuery
            {
                PageIndex = 1,
                PageSize = 5,
                SortOrder = "CreatedAt desc",
                SearchText = "test",
                BlogSiteId = blogSiteId
            };

            var expectedComments = new List<Comment>
            {
                new Comment { Id = Guid.NewGuid(), Name = "Comment 1", BlogsiteId = blogSiteId },
                new Comment { Id = Guid.NewGuid(), Name = "Comment 2", BlogsiteId = blogSiteId },
            };
            int total = 2;
            int totalDisplay = 2;

            // Mock repository method
            _commentRepositoryMock
                .Setup(r => r.GetCommentsForBlogSiteManagerAsync(
                    query.PageIndex,
                    query.PageSize,
                    query.SortOrder,
                    query.SearchText,
                    query.BlogSiteId, CancellationToken.None))
                .ReturnsAsync((expectedComments, total, totalDisplay))
                .Verifiable();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            this.ShouldSatisfyAllConditions(
                () => result.Item1.ShouldNotBeNull(),
                () => result.Item2.ShouldBe(expectedComments.Count),
                () => result.Item2.ShouldBe(total),
                () => result.Item3.ShouldBe(totalDisplay)
            );
        }
    }
}