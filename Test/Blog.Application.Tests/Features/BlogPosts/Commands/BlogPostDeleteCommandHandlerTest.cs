using Autofac.Extras.Moq;
using BlogSite.Application.Contracts;
using BlogSite.Application.Contracts.Repositories;
using BlogSite.Application.Features.BlogPosts.Commands;
using BlogSite.Domain;
using BlogSite.Domain.Entities;
using BlogSite.Domain.Enums;
using MapsterMapper;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace BlogSite.Application.Tests;

public class BlogPostDeleteCommandHandlerTest
{
    private AutoMock _mock;
    private BlogPostDeleteCommandHandler _handler;
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
        _handler = _mock.Create<BlogPostDeleteCommandHandler>();
    }

    [TearDown]
    public void TearDown()
    {
        _unitOfWorkMock.Reset();
        _blogPostRepositoryMock.Reset();
    }

    [Test]
    public async Task Handle_ValidId_SetsStatusInactive_AndSaves()
    {
        // Arrange
        var postId = Guid.NewGuid();

        var existingPost = new BlogPost
        {
            Id = postId,
            Title = "Test",
            Status = Status.Active
        };

        _blogPostRepositoryMock
            .Setup(x => x.GetByIdAsync(postId, CancellationToken.None))
            .ReturnsAsync(existingPost)
            .Verifiable();

        _blogPostRepositoryMock
            .Setup(x => x.RemoveAsync(It.Is<BlogPost>(p =>
                p.Id == postId &&
                p.Status == Status.Inactive), CancellationToken.None))
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

        var command = new BlogPostDeleteCommand
        {
            Id = postId
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        this.ShouldSatisfyAllConditions(
            () => result.ShouldBe(postId),
            () => existingPost.Status.ShouldBe(Status.Inactive),
            () => _unitOfWorkMock.VerifyAll(),
            () => _blogPostRepositoryMock.VerifyAll()
        );
    }
}