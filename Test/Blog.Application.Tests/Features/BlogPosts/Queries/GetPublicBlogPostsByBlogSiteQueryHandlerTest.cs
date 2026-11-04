using Autofac.Extras.Moq;
using BlogSite.Application.Contracts;
using BlogSite.Application.Contracts.Repositories;
using BlogSite.Application.Features.BlogPosts.Queries;
using BlogSite.Domain;
using BlogSite.Domain.Dtos;
using BlogSite.Domain.Entities;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace DevSkill.Application.Tests;

public class GetPublicBlogPostsByBlogSiteQueryHandlerTest
{
    private AutoMock _mock;
    private GetPublicBlogPostsByBlogSiteQueryHandler _handler;
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

        _handler = _mock.Create<GetPublicBlogPostsByBlogSiteQueryHandler>();
    }

    [TearDown]
    public void TearDown()
    {
        _unitOfWorkMock.Reset();
        _blogPostRepositoryMock.Reset();
    }

    [Test]
    public async Task Handle_ValidQuery_ReturnsPostsAndHasMore()
    {
        // Arrange
        var blogSiteId = Guid.NewGuid();

        var posts = new List<PublicBlogPostDto>
        {
            new PublicBlogPostDto
            {
                Id = Guid.NewGuid(),
                Author = "Author 1",
                AuthorSlag = "author-1",
                Title = "Post 1",
                ThumbnailPath = "thumb1.jpg",
                Tags = new List<Tag>(),
                Comments = new List<Comment>()
            },
            new PublicBlogPostDto
            {
                Id = Guid.NewGuid(),
                Author = "Author 2",
                AuthorSlag = "author-2",
                Title = "Post 2",
                ThumbnailPath = "thumb2.jpg",
                Tags = new List<Tag>(),
                Comments = new List<Comment>()
            }
        };

        bool expectedHasMore = true;

        var query = new GetPublicBlogPostsByBlogSiteQuery
        {
            PageIndex = 1,
            PageSize = 5,
            BlogSiteId = blogSiteId
        };

        _blogPostRepositoryMock
            .Setup(x => x.GetPublicBlogPostAsync(query.PageIndex, query.PageSize, query.BlogSiteId))
            .ReturnsAsync((posts.AsReadOnly(), expectedHasMore))
            .Verifiable();

        _unitOfWorkMock
            .SetupGet(x => x.BlogPostRepository)
            .Returns(_blogPostRepositoryMock.Object)
            .Verifiable();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        this.ShouldSatisfyAllConditions(
            () => result.Posts.ShouldNotBeNull(),
            () => result.Posts.Count.ShouldBe(posts.Count),
            () => result.HasMore.ShouldBeTrue(),
            () => result.Posts[0].Title.ShouldBe("Post 1"),
            () => result.Posts[1].Title.ShouldBe("Post 2"),
            () => _unitOfWorkMock.VerifyAll(),
            () => _blogPostRepositoryMock.VerifyAll()
        );
    }
}