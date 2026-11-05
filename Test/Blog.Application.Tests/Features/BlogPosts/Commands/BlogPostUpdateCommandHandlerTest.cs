using Autofac.Extras.Moq;
using BlogSite.Application.Contracts;
using BlogSite.Application.Contracts.Repositories;
using BlogSite.Application.Features.BlogPosts.Commands;
using BlogSite.Domain;
using BlogSite.Domain.Entities;
using MapsterMapper;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace Blog.Application.Tests;

public class BlogPostUpdateCommandHandlerTest
{
    private AutoMock _mock;
    private BlogPostUpdateCommandHandler _handler;
    private Mock<IApplicationUnitOfWork> _unitOfWorkMock;
    private Mock<IBlogPostRepository> _blogPostRepositoryMock;
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
        _blogPostRepositoryMock = _mock.Mock<IBlogPostRepository>();
        _mapperMock = _mock.Mock<IMapper>();

        _handler = _mock.Create<BlogPostUpdateCommandHandler>();
    }

    [TearDown]
    public void TearDown()
    {
        _unitOfWorkMock.Reset();
        _blogPostRepositoryMock.Reset();
        _mapperMock.Reset();
    }

    [Test]
    public async Task Handle_ValidCommand_UpdatesPostAndReplacesCategoriesAndTags()
    {
        // Arrange
        var postId = Guid.NewGuid();

        var existingPost = new BlogPost
        {
            Id = postId,
            Title = "Old Title",
            Body = "Old Body",
            CreatedAt = DateTime.UtcNow.AddDays(-1),
            UpdatedAt = DateTime.UtcNow,
            BlogPostCategories = new List<BlogPostCategory>
            {
                new BlogPostCategory { CategoryId = Guid.NewGuid() }
            },
            BlogPostTags = new List<BlogPostTag>
            {
                new BlogPostTag { TagId = Guid.NewGuid() }
            }
        };

        var newCategoryIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
        var newTagIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };

        var command = new BlogPostUpdateCommand
        {
            Id = postId,
            Title = "New Title",
            Body = "New Body",
            ThumbnailPath = "thumb.jpg",
            Categories = newCategoryIds,
            Tags = newTagIds,
            UpdatedAt = DateTime.UtcNow
        };

        _blogPostRepositoryMock
            .Setup(x => x.GetByIdAsync(postId, CancellationToken.None))
            .ReturnsAsync(existingPost)
            .Verifiable();

        _mapperMock
            .Setup(x => x.Map(command, existingPost))
            .Verifiable();

        _blogPostRepositoryMock
            .Setup(x => x.EditAsync(It.Is<BlogPost>(p =>
                p.Id == postId &&
                p.BlogPostCategories.Count == newCategoryIds.Count &&
                p.BlogPostTags.Count == newTagIds.Count
            ), CancellationToken.None))
            .Returns(Task.CompletedTask)
            .Verifiable();

        _unitOfWorkMock
            .SetupGet(x => x.BlogPostRepository)
            .Returns(_blogPostRepositoryMock.Object)
            .Verifiable();

        _unitOfWorkMock
            .Setup(x => x.SaveAsync(CancellationToken.None))
            .Returns(Task.CompletedTask)
            .Verifiable();

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        this.ShouldSatisfyAllConditions(
            () => result.ShouldNotBeNull(),
            () => result.Id.ShouldBe(postId),
            () => result.BlogPostCategories.Count.ShouldBe(newCategoryIds.Count),
            () => result.BlogPostTags.Count.ShouldBe(newTagIds.Count),
            () => _mapperMock.Verify(x => x.Map(command, existingPost), Times.Once),
            () => _unitOfWorkMock.VerifyAll(),
            () => _blogPostRepositoryMock.VerifyAll()
        );
    }
}