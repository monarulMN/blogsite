using Autofac.Extras.Moq;
using BlogSite.Application.Contracts;
using BlogSite.Application.Contracts.Repositories;
using BlogSite.Application.Features.BlogPosts.Queries;
using BlogSite.Domain;
using BlogSite.Domain.Entities;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace DevSkill.Application.Tests;

public class GetBlogPostsAdminQueryHandlerTest
{
    private AutoMock _mock;
    private GetBlogPostsAdminQueryHandler _handler;
    private Mock<IApplicationUnitOfWork> _unitOfWorkMock;
    private Mock<IBlogPostRepository> _blogPostRepositoryMock;

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
        _blogPostRepositoryMock = _mock.Mock<IBlogPostRepository>();

        _handler = _mock.Create<GetBlogPostsAdminQueryHandler>();
    }

    [TearDown]
    public void TearDown()
    {
        _unitOfWorkMock.Reset();
        _blogPostRepositoryMock.Reset();
    }

    [Test]
    public async Task Handle_ValidQuery_ReturnsPagedResult()
    {
        // Arrange
        var posts = new List<BlogPost>
        {
            new BlogPost { Id = Guid.NewGuid(), Title = "Post 1" },
            new BlogPost { Id = Guid.NewGuid(), Title = "Post 2" }
        };

        var expectedTotal = 10;
        var expectedTotalDisplay = 2;

        var query = new GetBlogPostsAdminQuery
        {
            PageSize = 5,
            PageIndex = 1,
            SortOrder = "title_asc",
            SearchText = "Post"
        };

        _blogPostRepositoryMock
            .Setup(x => x.GetPagedAdminBlogPostsAsync(
                query.PageSize,
                query.PageIndex,
                query.SortOrder,
                query.SearchText), CancellationToken.None)
            .ReturnsAsync((posts, expectedTotal, expectedTotalDisplay))
            .Verifiable();

        _unitOfWorkMock
            .SetupGet(x => x.BlogPostRepository)
            .Returns(_blogPostRepositoryMock.Object)
            .Verifiable();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        this.ShouldSatisfyAllConditions(
            () => result.Item1.ShouldNotBeNull(),
            () => result.Item1.Count.ShouldBe(posts.Count),
            () => result.Item2["Total"].ShouldBe(expectedTotal),
            () => result.Item2["TotalDisplay"].ShouldBe(expectedTotalDisplay),
            () => _unitOfWorkMock.VerifyAll(),
            () => _blogPostRepositoryMock.VerifyAll()
        );
    }
}