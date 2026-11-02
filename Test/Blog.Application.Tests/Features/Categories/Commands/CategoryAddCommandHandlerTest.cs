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
    public class CategoryAddCommandHandlerTest
    {
        private AutoMock _mock;
        private CategoryAddCommandHandler _handler;
        private Mock<IApplicationUnitOfWork> _unitOfWorkMock;
        private Mock<ICategoryRepository> _categoryRepositoryMock;
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
            _categoryRepositoryMock = _mock.Mock<ICategoryRepository>();
            _mapperMock = _mock.Mock<IMapper>();

            _unitOfWorkMock.Setup(x => x.CategoryRepository)
                .Returns(_categoryRepositoryMock.Object);

            _handler = _mock.Create<CategoryAddCommandHandler>();
        }

        [TearDown]
        public void TearDown()
        {
            _unitOfWorkMock.Reset();
            _categoryRepositoryMock.Reset();
            _mapperMock.Reset();
        }

        [Test]
        public async Task Handle_ShouldAddCategory_WhenCommandIsValid()
        {
            // Arrange
            var command = new CategoryAddCommand
            {
                Name = "Test Category",
                BlogSiteId = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var categoryEntity = new Category
            {
                Name = command.Name,
                BlogSiteId = command.BlogSiteId,
                CreatedAt = command.CreatedAt,
                UpdatedAt = command.UpdatedAt
            };

            //Mapper setup
            _mapperMock
                .Setup(m => m.Map<Category>(command))
                .Returns(categoryEntity)
                .Verifiable();

            //Repository AddAsync setup
            _categoryRepositoryMock
                .Setup(r => r.AddAsync(categoryEntity, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            //SaveAsync setup
            _unitOfWorkMock
                .Setup(u => u.SaveAsync(It.IsAny<CancellationToken>()))
                .Verifiable();

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            this.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.ShouldBeOfType<Category>(),
                () => result.Id.ShouldNotBe(Guid.Empty),
                () => result.Name.ShouldBe(command.Name),
                () => result.BlogSiteId.ShouldBe(command.BlogSiteId),
                () => _mapperMock.VerifyAll(),
                () => _categoryRepositoryMock.VerifyAll(),
                () => _unitOfWorkMock.VerifyAll()
            );
        }
    }
}
