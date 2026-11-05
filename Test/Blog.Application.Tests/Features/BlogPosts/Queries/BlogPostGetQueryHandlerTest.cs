using Autofac.Extras.Moq;
using BlogSite.Application.Contracts;
using BlogSite.Application.Contracts.Repositories;
using BlogSite.Application.Features.BlogPosts.Queries;
using BlogSite.Domain;
using BlogSite.Domain.Entities;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace Blog.Application.Tests;

public class GetBlogPostQueryHandlerTest
{
    private AutoMock _mock;
    private GetBlogPostQueryHandler _handler;
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

        _handler = _mock.Create<GetBlogPostQueryHandler>();
    }

    [TearDown]
    public void TearDown()
    {
        _unitOfWorkMock.Reset();
        _blogPostRepositoryMock.Reset();
    }

    [Test]
    public async Task Handle_ValidId_ReturnsBlogPost()
    {
        // Arrange
        var postId = Guid.NewGuid();

        var expectedPost = new BlogPost
        {
            Id = postId,
            Title = "Test Title",
            Body = "Test Body"
        };

        _blogPostRepositoryMock
            .Setup(x => x.GetByIdAsync(postId, CancellationToken.None))
            .ReturnsAsync(expectedPost)
            .Verifiable();

        _unitOfWorkMock
            .SetupGet(x => x.BlogPostRepository)
            .Returns(_blogPostRepositoryMock.Object)
            .Verifiable();

        var query = new GetBlogPostQuery
        {
            Id = postId
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        this.ShouldSatisfyAllConditions(
            () => result.ShouldNotBeNull(),
            () => result.ShouldBeOfType<BlogPost>(),
            () => result.Id.ShouldBe(postId),
            () => result.Title.ShouldBe(expectedPost.Title),
            () => result.Body.ShouldBe(expectedPost.Body),
            () => _unitOfWorkMock.VerifyAll(),
            () => _blogPostRepositoryMock.VerifyAll()
        );
    }
}