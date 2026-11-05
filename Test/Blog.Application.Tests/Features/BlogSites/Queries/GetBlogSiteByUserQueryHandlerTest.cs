using Autofac.Extras.Moq;
using BlogSite.Application.Contracts;
using BlogSite.Application.Contracts.Repositories;
using BlogSite.Application.Features.BlogSites.Queries;
using BlogSite.Domain;
using BlogSite.Domain.Entities;
using Moq;
using NUnit.Framework;
using Shouldly;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Application.Tests;

public class GetBlogSiteByUserQueryHandlerTest
{
    private AutoMock _mock;
    private GetBlogSiteByUserQueryHandler _handler;
    private Mock<IApplicationUnitOfWork> _unitOfWorkMock;
    private Mock<IBlogSiteRepository> _blogSiteRepositoryMock;

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
        _blogSiteRepositoryMock = _mock.Mock<IBlogSiteRepository>();
        _unitOfWorkMock.SetupGet(x => x.BlogSiteRepository)
            .Returns(_blogSiteRepositoryMock.Object)
            .Verifiable();

        _handler = _mock.Create<GetBlogSiteByUserQueryHandler>();
    }

    [TearDown]
    public void TearDown()
    {
        _unitOfWorkMock.Reset();
        _blogSiteRepositoryMock.Reset();
    }

    [Test]
    public async Task Handle_ValidAuthorId_ReturnsBlogSite()
    {
        // Arrange
        var authorId = Guid.NewGuid();
        var query = new GetBlogSiteByUserQuery { AuthorId = authorId };

        var blogSite = new Blogsite
        {
            Id = Guid.NewGuid(),
            Name = "Author's Blog",
            Description = "A blog by author",
            Slug = "authors-blog",
            CreatedAt = DateTime.UtcNow.AddDays(-5)
        };

        _blogSiteRepositoryMock
            .Setup(x => x.GetByAuthorIdAsync(authorId, CancellationToken.None))
            .ReturnsAsync(blogSite)
            .Verifiable();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        this.ShouldSatisfyAllConditions(
            () => result.ShouldNotBeNull(),
            () => result.ShouldBeOfType<Blogsite>(),
            () => result.Id.ShouldBe(blogSite.Id),
            () => result.Name.ShouldBe(blogSite.Name),
            () => _blogSiteRepositoryMock.VerifyAll()
        );
    }

    [Test]
    public async Task Handle_NonExistentAuthorId_ReturnsNull()
    {
        // Arrange
        var authorId = Guid.NewGuid();
        var query = new GetBlogSiteByUserQuery { AuthorId = authorId };

        _blogSiteRepositoryMock
            .Setup(x => x.GetByAuthorIdAsync(authorId, CancellationToken.None))
            .ReturnsAsync((Blogsite)null)
            .Verifiable();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        this.ShouldSatisfyAllConditions(
            () => result.ShouldBeNull(),
            () => _blogSiteRepositoryMock.VerifyAll()
        );
    }
}