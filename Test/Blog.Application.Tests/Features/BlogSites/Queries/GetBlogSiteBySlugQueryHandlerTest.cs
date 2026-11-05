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

public class GetBlogSiteBySlugQueryHandlerTest
{
    private AutoMock _mock;
    private GetBlogSiteBySlugQueryHandler _handler;
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

        _handler = _mock.Create<GetBlogSiteBySlugQueryHandler>();
    }

    [TearDown]
    public void TearDown()
    {
        _unitOfWorkMock.Reset();
        _blogSiteRepositoryMock.Reset();
    }

    [Test]
    public async Task Handle_ValidSlug_ReturnsBlogSite()
    {
        // Arrange
        var slug = "my-blog-site";
        var query = new GetBlogSiteBySlugQuery { Slug = slug };

        var blogSite = new Blogsite
        {
            Id = Guid.NewGuid(),
            Name = "My Blog Site",
            Description = "A test blog site",
            Slug = slug
        };

        _blogSiteRepositoryMock
            .Setup(x => x.GetBlogSiteBySlugAsync(slug, CancellationToken.None))
            .ReturnsAsync(blogSite)
            .Verifiable();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        this.ShouldSatisfyAllConditions(
            () => result.ShouldNotBeNull(),
            () => result.ShouldBeOfType<Blogsite>(),
            () => result.Slug.ShouldBe(slug),
            () => result.Name.ShouldBe(blogSite.Name),
            () => _blogSiteRepositoryMock.VerifyAll()
        );
    }

    [Test]
    public async Task Handle_NonExistentSlug_ReturnsNull()
    {
        // Arrange
        var slug = "non-existent-slug";
        var query = new GetBlogSiteBySlugQuery { Slug = slug };

        _blogSiteRepositoryMock
            .Setup(x => x.GetBlogSiteBySlugAsync(slug, CancellationToken.None))
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