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

namespace BlogSite.Application.Tests;

public class GetPublicBlogPostGetWithTagQueryHandlerTest
{
    private AutoMock _mock;
    private GetPublicBlogPostGetWithTagQueryHandler _handler;
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

        _handler = _mock.Create<GetPublicBlogPostGetWithTagQueryHandler>();
    }

    [TearDown]
    public void TearDown()
    {
        _unitOfWorkMock.Reset();
        _blogPostRepositoryMock.Reset();
    }

    [Test]
    public async Task Handle_ValidBlogPostId_ReturnsPublicBlogPostDto()
    {
        // Arrange
        var blogPostId = Guid.NewGuid();

        var expectedDto = new PublicBlogPostDto
        {
            Id = blogPostId,
            Author = "John Doe",
            AuthorSlag = "john-doe",
            Title = "Test Title",
            Body = "Test Body",
            ThumbnailPath = "thumb.jpg",
            CreatedAt = DateTime.UtcNow,
            Tags = new List<Tag>
            {
                new Tag { Id = Guid.NewGuid(), Name = "CSharp" }
            },
            Comments = new List<Comment>
            {
                new Comment { Id = Guid.NewGuid(), Text = "Nice post!" }
            }
        };

        _blogPostRepositoryMock
            .Setup(x => x.GetPublicBlogPostWithTagAsync(blogPostId))
            .ReturnsAsync(expectedDto)
            .Verifiable();

        _unitOfWorkMock
            .SetupGet(x => x.BlogPostRepository)
            .Returns(_blogPostRepositoryMock.Object)
            .Verifiable();

        var query = new GetPublicBlogPostGetWithTagQuery
        {
            BlogPostId = blogPostId
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        this.ShouldSatisfyAllConditions(
            () => result.ShouldNotBeNull(),
            () => result.ShouldBeOfType<PublicBlogPostDto>(),
            () => result.Id.ShouldBe(blogPostId),
            () => result.Author.ShouldBe(expectedDto.Author),
            () => result.Tags.Count.ShouldBe(1),
            () => result.Comments.Count.ShouldBe(1),
            () => _unitOfWorkMock.VerifyAll(),
            () => _blogPostRepositoryMock.VerifyAll()
        );
    }
}