using Autofac.Extras.Moq;
using BlogSite.Application.Contracts;
using BlogSite.Application.Contracts.Repositories;
using BlogSite.Application.Features.BlogSites.Queries;
using BlogSite.Domain;
using BlogSite.Domain.Dtos;
using Moq;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Application.Tests;

public class GetAllBlogSitesQueryHandlerTest
{
    private AutoMock _mock;
    private GetAllBlogSitesQueryHandler _handler;
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
        _unitOfWorkMock.SetupGet(x => x.BlogSiteRepository).Returns(_blogSiteRepositoryMock.Object);

        _handler = _mock.Create<GetAllBlogSitesQueryHandler>();
    }

    [TearDown]
    public void TearDown()
    {
        _unitOfWorkMock.Reset();
        _blogSiteRepositoryMock.Reset();
    }

    [Test]
    public async Task Handle_ValidQuery_ReturnsPagedBlogSites()
    {
        // Arrange
        var query = new GetAllBlogSitesQuery
        {
            PageIndex = 1,
            PageSize = 5,
            SortOrder = "Name",
            SearchText = "Test"
        };

        var blogSites = new List<BlogSiteDto>
        {
            new BlogSiteDto 
            { 
                Id = Guid.NewGuid(), 
                Name = "Blog1", 
                UserName = "User1", 
                Email = "user1@test.com", 
                TotalCategory = 3, 
                TotalPost = 10, 
                CreatedAt = DateTime.UtcNow.AddDays(-5) },

            new BlogSiteDto 
            { 
                Id = Guid.NewGuid(), 
                Name = "Blog2", 
                UserName = "User2", 
                Email = "user2@test.com", 
                TotalCategory = 2, 
                TotalPost = 5, 
                CreatedAt = DateTime.UtcNow.AddDays(-3) }
        };

        int total = 2;
        int totalDisplay = 2;

        _blogSiteRepositoryMock
            .Setup(x => x.GetPagedBlogSiteAsync(
                query.PageSize, 
                query.PageIndex, 
                query.SortOrder, 
                query.SearchText,
                CancellationToken.None))
            .ReturnsAsync((blogSites, total, totalDisplay))
            .Verifiable();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        this.ShouldSatisfyAllConditions(
            () => result.Item1.ShouldNotBeNull(),
            () => result.Item2.ShouldBe(total),
            () => result.Item3.ShouldBe(totalDisplay),
            () => result.Item1[0].Name.ShouldBe("Blog1"),
            () => result.Item1[1].Name.ShouldBe("Blog2"),
            () => _blogSiteRepositoryMock.VerifyAll()
        );
    }

    [Test]
    public async Task Handle_EmptyResult_ReturnsEmptyList()
    {
        // Arrange
        var query = new GetAllBlogSitesQuery
        {
            PageIndex = 1,
            PageSize = 5,
            SortOrder = "Name",
            SearchText = "NonExisting"
        };

        _blogSiteRepositoryMock
            .Setup(x => x.GetPagedBlogSiteAsync(
                query.PageSize, 
                query.PageIndex, 
                query.SortOrder, 
                query.SearchText, 
                CancellationToken.None))
            .ReturnsAsync((new List<BlogSiteDto>(), 0, 0))
            .Verifiable();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        this.ShouldSatisfyAllConditions(
            () => result.Item1.ShouldNotBeNull(),
            () => result.Item1.Count.ShouldBe(0),
            () => result.Item2.ShouldBe(0),
            () => result.Item3.ShouldBe(0),
            () => _blogSiteRepositoryMock.VerifyAll()
        );
    }
}