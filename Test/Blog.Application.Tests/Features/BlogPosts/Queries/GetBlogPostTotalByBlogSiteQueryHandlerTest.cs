using Autofac.Extras.Moq;
using BlogSite.Application.Contracts;
using BlogSite.Application.Contracts.Repositories;
using BlogSite.Application.Features.BlogPosts.Queries;
using BlogSite.Domain;
using BlogSite.Domain.Entities;
using BlogSite.Domain.Enums;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace DevSkill.Application.Tests;

public class GetBlogPostTotalByBlogSiteQueryHandlerTest
{
    private AutoMock _mock;
    private GetBlogPostTotalByBlogSiteQueryHandler _handler;
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

        _handler = _mock.Create<GetBlogPostTotalByBlogSiteQueryHandler>();
    }

    [TearDown]
    public void TearDown()
    {
        _unitOfWorkMock.Reset();
        _blogPostRepositoryMock.Reset();
    }

    [Test]
    public async Task Handle_ValidBlogSiteId_ReturnsPublishedAndDraftCounts()
    {
        // Arrange
        var blogSiteId = Guid.NewGuid();

        var query = new GetBlogPostTotalByBlogSiteQuery
        {
            BlogSiteId = blogSiteId
        };

        _blogPostRepositoryMock
            .SetupSequence(x => x.GetCountAsync(It.IsAny<System.Linq.Expressions.Expression<Func<BlogPost, bool>>>()))
            .ReturnsAsync(5)   // Published count
            .ReturnsAsync(3);  // Draft count

        _unitOfWorkMock
            .SetupGet(x => x.BlogPostRepository)
            .Returns(_blogPostRepositoryMock.Object);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        this.ShouldSatisfyAllConditions(
            () => result.Item1.ShouldBe(5),
            () => result.Item2.ShouldBe(3),
            () => _blogPostRepositoryMock.VerifyAll()
        );
    }
}