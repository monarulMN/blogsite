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

namespace BlogSite.Application.Tests;

public class BlogPostAddCommandHandlerTest
{
    private AutoMock _mock;
    private BlogPostAddCommandHandler _handler;
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

        _handler = _mock.Create<BlogPostAddCommandHandler>();
    }

    [TearDown]
    public void TearDown()
    {
        _unitOfWorkMock.Reset();
        _blogPostRepositoryMock.Reset();
        _mapperMock.Reset();
    }

    [Test]
    public async Task Handle_ValidCommand_AddsBlogPostWithCategoriesAndTags()
    {
        // Arrange
        var categoryIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
        var tagIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };

        var command = new BlogPostAddCommand
        {
            Title = "Test Title",
            Body = "Test Content",
            ThumbnailPath = "img.jpg",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Categories = categoryIds,
            Tags = tagIds
        };

        var mappedBlogPost = new BlogPost
        {
            Title = command.Title,
            Body = command.Body,
            ThumbnailPath = command.ThumbnailPath,
            BlogPostCategories = new List<BlogPostCategory>(),
            BlogPostTags = new List<BlogPostTag>()
        };

        _mapperMock
            .Setup(x => x.Map<BlogPost>(command))
            .Returns(mappedBlogPost);

        _blogPostRepositoryMock
            .Setup(x => x.AddAsync(It.Is<BlogPost>(bp =>
                bp.Title == command.Title &&
                bp.Body == command.Body &&
                bp.Id != Guid.Empty &&
                bp.BlogPostCategories.Count == categoryIds.Count &&
                bp.BlogPostTags.Count == tagIds.Count
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
            () => result.ShouldBeOfType<BlogPost>(),
            () => result.Title.ShouldBe(command.Title),
            () => result.Body.ShouldBe(command.Body),
            () => result.Id.ShouldNotBe(Guid.Empty),
            () => result.BlogPostCategories.Count.ShouldBe(categoryIds.Count),
            () => result.BlogPostTags.Count.ShouldBe(tagIds.Count),
            () => _unitOfWorkMock.VerifyAll(),
            () => _blogPostRepositoryMock.VerifyAll()
        );
    }
}