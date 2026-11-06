using Autofac.Extras.Moq;
using BlogSite.Application.Contracts;
using BlogSite.Application.Contracts.Repositories;
using BlogSite.Application.Features.Categories.Commands;
using BlogSite.Domain.Entities;
using MapsterMapper;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Application.Tests.Features.Categories.Commands
{
    public class CategoryUpdateCommandHandlerTest
    {
        private AutoMock _mock;
        private CategoryUpdateCommandHandler _handler;
        private Mock<IApplicationUnitOfWork> _mockUnitOfWork;
        private Mock<ICategoryRepository> _mockCategoryRepository;
        private Mock<IMapper> _mockMapper;


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
            _mockCategoryRepository = _mock.Mock<ICategoryRepository>();
            _mockMapper = _mock.Mock<IMapper>();

            _mockUnitOfWork.SetupGet(x => x.CategoryRepository)
                .Returns(_mockCategoryRepository.Object);

            _handler = _mock.Create<CategoryUpdateCommandHandler>();
        }

        [TearDown]
        public void TearDown()
        {
            _mockUnitOfWork.Reset();
            _mockCategoryRepository.Reset();
            _mockMapper.Reset();
        }

        [Test]
        public async Task Handle_ValidComand_UpdateCategory_PreservesBlogSiteId()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var blogSiteId = Guid.NewGuid();

            var command = new CategoryUpdateCommand
            {
                Id = categoryId,
                Name = "Updated Category",
                UpdatedAt = DateTime.UtcNow
            };
            var existingCategory = new Category
            {
                Id = categoryId,
                Name = "Old Category",
                BlogsiteId = blogSiteId,
                CreatedAt = DateTime.UtcNow.AddDays(-4),
                UpdatedAt = DateTime.UtcNow.AddDays(-4)
            };

            //Mock getByIdAsync
            _mockCategoryRepository
                .Setup(x => x.GetByIdAsync(categoryId, CancellationToken.None))
                .ReturnsAsync(existingCategory)
                .Verifiable();
            
            //Mock Mapper.Map(command, entity) to actually update properties
            _mockMapper.Setup(x => x.Map(command, existingCategory))
                .Callback<CategoryUpdateCommand, Category>((cmd, cate) =>
                {
                    cate.Name = cmd.Name;
                    cate.UpdatedAt = cmd.UpdatedAt;
                })
                .Returns(existingCategory)
                .Verifiable();

            //Mock EditAsync
            _mockCategoryRepository
                .Setup(x => x.EditAsync(existingCategory, CancellationToken.None))
                .Returns(Task.CompletedTask)
                .Verifiable();

            //Mock SaveAsync
            _mockUnitOfWork
                .Setup(x => x.SaveAsync(CancellationToken.None))
                .Returns(Task.CompletedTask)
                .Verifiable();

            // Act
            var result =await _handler.Handle(command, CancellationToken.None);

            // Assert
            this.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.ShouldBeOfType<Category>(),
                () => result.Id.ShouldBe(categoryId),
                () => result.Name.ShouldBe(command.Name),
                () => result.BlogsiteId.ShouldBe(blogSiteId),
                () => result.UpdatedAt.ShouldBe(command.UpdatedAt),
                () => _mockCategoryRepository.VerifyAll(),
                () => _mockUnitOfWork.VerifyAll(),
                () => _mockMapper.VerifyAll()                
            );
        }
    }
}
