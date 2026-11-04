using Autofac.Extras.Moq;
using BlogSite.Application.Contracts;
using BlogSite.Application.Contracts.Repositories;
using BlogSite.Application.Features.BlogSites.Commands;
using BlogSite.Domain.Entities;
using MapsterMapper;
using Moq;
using Shouldly;
using Slugify;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Application.Tests.Features.BlogSites.Commands
{
    public class BlogSiteAddCommandHandlerTest
    {
        private AutoMock _mock;
        private BlogSiteAddCommandHandler _handler;
        private Mock<IApplicationUnitOfWork> _mockUnitOfWork;
        private Mock<IBlogSiteRepository> _mockBlogSiteRepository;
        private Mock<IMapper> _mockMapper;
        private Mock<ISlugHelper> _mockSlugHelper;


        [OneTimeSetUp]
        public void OneTimeSetUp()
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

            _handler = new BlogSiteAddCommandHandler(
                _mockUnitOfWork.Object,
                _mockMapper.Object,
                _mockSlugHelper.Object);
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
        public async Task Handle_ValidCommand_AddsBlogSite()
        {
            //Arrange
            var command = new BlogSiteAddCommand
            {
                Name = "Test Blog Site",
                Description = "Test Description",
                AuthorId = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var mappedBlogSite = new Blogsite
            {
                Name = command.Name,
                Description = command.Description,
                AuthorId = command.AuthorId,
                CreatedAt = command.CreatedAt,
                UpdatedAt = command.UpdatedAt
            };

            _mockMapper.Setup(m => m.Map<Blogsite>(command)).Returns(mappedBlogSite);

            var generatedSlug = "test-blog-site" + Guid.NewGuid().ToString("N");
            _mockSlugHelper.Setup(s => s.GenerateSlug(It.IsAny<string>())).Returns(generatedSlug);

            _mockBlogSiteRepository.Setup(x => x.AddAsync(
                It.Is<Blogsite>(b =>
                b.Name == command.Name &&
                b.Description == command.Description &&
                b.Id != Guid.Empty &&
                b.Slug == generatedSlug),
                CancellationToken.None))
                .Returns(Task.CompletedTask)
                .Verifiable();

            _mockUnitOfWork.Setup(x => x.SaveAsync(CancellationToken.None))
                .Returns(Task.CompletedTask)
                .Verifiable();

            _mockUnitOfWork.SetupGet(x => x.BlogSiteRepository)
                .Returns(_mockBlogSiteRepository.Object)
                .Verifiable();

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            this.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.ShouldBeOfType<Blogsite>(),
                () => result.Name.ShouldBe(command.Name),
                () => result.Description.ShouldBe(command.Description),
                () => result.Slug.ShouldBe(generatedSlug),
                () => result.Id.ShouldNotBe(Guid.Empty),
                () => _mockUnitOfWork.VerifyAll(),
                () => _mockBlogSiteRepository.VerifyAll()
            );

        }
    }
}
