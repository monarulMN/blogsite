using Autofac.Extras.Moq;
using BlogSite.Application.Contracts;
using BlogSite.Application.Contracts.Repositories;
using BlogSite.Application.Features.BlogSites.Commands;
using BlogSite.Domain;
using BlogSite.Domain.Entities;
using MapsterMapper;
using Moq;
using NUnit.Framework;
using Shouldly;
using Slugify;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Application.Tests;

public class BlogSiteUpdateCommandHandlerTest
{
    private AutoMock _mock;
    private BlogSiteUpdateCommandHandler _handler;
    private Mock<IApplicationUnitOfWork> _mockUnitOfWork;
    private Mock<IBlogSiteRepository> _mockBlogSiteRepository;
    private Mock<IMapper> _mockMapper;
    private Mock<ISlugHelper> _mockSlugHelper;

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
        _mockUnitOfWork = _mock.Mock<IApplicationUnitOfWork>();
        _mockBlogSiteRepository = _mock.Mock<IBlogSiteRepository>();
        _mockMapper = _mock.Mock<IMapper>();
        _mockSlugHelper = _mock.Mock<ISlugHelper>();

        _handler = _mock.Create<BlogSiteUpdateCommandHandler>();
    }

    [TearDown]
    public void TearDown()
    {
        _mockUnitOfWork.Reset();
        _mockBlogSiteRepository.Reset();
        _mockMapper.Reset();
        _mockSlugHelper.Reset();
    }

    [Test]
    public async Task Handle_ValidCommand_UpdatesBlogSite()
    {
        // Arrange
        var blogSiteId = Guid.NewGuid();

        var command = new BlogSiteUpdateCommand
        {
            Id = blogSiteId,
            Name = "Updated Blog Site",
            Description = "Updated Description",
            UpdatedAt = DateTime.UtcNow
        };

        var existingBlogSite = new Blogsite
        {
            Id = blogSiteId,
            Name = "Old Name",
            Description = "Old Description",
            CreatedAt = DateTime.UtcNow.AddDays(-1),
            UpdatedAt = DateTime.UtcNow.AddDays(-1)
        };

        // Mock GetByIdAsync
        _mockBlogSiteRepository
            .Setup(x => x.GetByIdAsync(blogSiteId, CancellationToken.None))
            .ReturnsAsync(existingBlogSite)
            .Verifiable();

        // Mock Mapper.Map(command, entity) and update the entity
        _mockMapper
            .Setup(x => x.Map(command, existingBlogSite))
            .Callback<BlogSiteUpdateCommand, Blogsite>((cmd, blog) => {
                blog.Name = cmd.Name;
                blog.Description = cmd.Description;
                blog.UpdatedAt = cmd.UpdatedAt;
            })
            .Returns(existingBlogSite)
            .Verifiable();

        // Mock SlugHelper
        var generatedSlug = "updated-blog-site-" + blogSiteId.ToString("N");
        _mockSlugHelper
            .Setup(x => x.GenerateSlug($"{command.Name}-{existingBlogSite.Id}"))
            .Returns(generatedSlug)
            .Verifiable();

        // Mock EditAsync
        _mockBlogSiteRepository
            .Setup(x => x.EditAsync(existingBlogSite, CancellationToken.None))
            .Returns(Task.CompletedTask)
            .Verifiable();

        // Mock SaveAsync
        _mockUnitOfWork
            .Setup(x => x.SaveAsync(CancellationToken.None))
            .Returns(Task.CompletedTask)
            .Verifiable();

        _mockUnitOfWork
            .SetupGet(x => x.BlogSiteRepository)
            .Returns(_mockBlogSiteRepository.Object)
            .Verifiable();

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        this.ShouldSatisfyAllConditions(
            () => result.ShouldNotBeNull(),
            () => result.ShouldBeOfType<Blogsite>(),
            () => result.Id.ShouldBe(blogSiteId),
            () => result.Slug.ShouldBe(generatedSlug),
            () => result.Name.ShouldBe(command.Name),
            () => result.Description.ShouldBe(command.Description),
            () => _mockBlogSiteRepository.VerifyAll(),
            () => _mockUnitOfWork.VerifyAll(),
            () => _mockMapper.VerifyAll(),
            () => _mockSlugHelper.VerifyAll()
        );
    }
}