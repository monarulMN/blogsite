using Autofac.Extras.Moq;
using BlogSite.Application.Contracts;
using BlogSite.Application.Contracts.Repositories;
using BlogSite.Application.Features.Categories.Commands;
using BlogSite.Domain.Entities;
using BlogSite.Domain.Enums;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Application.Tests.Features.Categories.Commands
{
    public class CategoryDeleteCommandHanlderTest
    {
        private AutoMock _mock;
        private CategoryDeleteCommandHandler _handler;
        private Mock<IApplicationUnitOfWork> _mockUnitOfWork;
        private Mock<ICategoryRepository> _mockCategoryRepository;

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

            _mockUnitOfWork.Setup(x => x.CategoryRepository)
                .Returns(_mockCategoryRepository.Object);

            _handler = _mock.Create<CategoryDeleteCommandHandler>();
        }

        [TearDown]
        public void TearDown()
        {
            _mockUnitOfWork.Reset();
            _mockCategoryRepository.Reset();
        }

        [Test]
        public async Task Handle_ValidCommand_MarksCategoryInactive()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            
            var category = new Category
            {
                Id = categoryId,
                Name = "Test Category",
                Status = Status.Active
            };

            var command = new CategoryDeleteCommand
            {
                Id = categoryId
            };

            //Mock GetByIdAsync
            _mockCategoryRepository
                .Setup(x => x.GetByIdAsync(categoryId, CancellationToken.None))
                .ReturnsAsync(category)
                .Verifiable();

            //Mock RemoveAsync
            _mockCategoryRepository
                .Setup(x => x.RemoveAsync(category, CancellationToken.None))
                .Returns(Task.CompletedTask)
                .Verifiable();

            //Mock SaveAsync
            _mockUnitOfWork
                .Setup(x => x.SaveAsync(CancellationToken.None))
                .Returns(Task.CompletedTask)
                .Verifiable();

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
           this.ShouldSatisfyAllConditions(
               () => result.ShouldBe(categoryId),
               () => category.Status.ShouldBe(Status.Inactive),
               () => _mockCategoryRepository.VerifyAll(),
               () => _mockUnitOfWork.VerifyAll()
               );
        }

    }
}
