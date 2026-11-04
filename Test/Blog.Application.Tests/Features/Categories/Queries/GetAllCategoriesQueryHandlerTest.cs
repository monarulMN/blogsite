using Autofac.Extras.Moq;
using BlogSite.Application.Contracts;
using BlogSite.Application.Contracts.Repositories;
using BlogSite.Application.Features.Categories.Queries;
using BlogSite.Domain.Entities;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Application.Tests.Features.Categories.Queries
{
    public class GetAllCategoriesQueryHandlerTest
    {
        private AutoMock _mock;
        private GetAllCategoriesQueryHandler _handler;
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

            _mockUnitOfWork.SetupGet(x => x.CategoryRepository)
                .Returns(_mockCategoryRepository.Object);

            _handler = _mock.Create<GetAllCategoriesQueryHandler>();
        }

        [TearDown]
        public void TearDown()
        {
            _mockUnitOfWork.Reset();
            _mockCategoryRepository.Reset();
        }

        [Test]
        public async Task Handle_ValidQuery_ReturnsPagedCategorieseForBlogSite()
        {
            //Arrange
            var blogSiteId = Guid.NewGuid();
            var categories = new List<Category>
            {
                new Category { Id = Guid.NewGuid(), Name = "Category 1", BlogSiteId = blogSiteId},
                new Category { Id = Guid.NewGuid(), Name = "Category 2", BlogSiteId = blogSiteId}
            };

            var query = new GetAllCategoriesQuery
            {
                PageIndex = 1,
                PageSize = 10,
                SortOrder = "Name",
                SearchText = null,
                BlogSiteId = blogSiteId
            };

            var expectedTotal = 2;
            var expectedTotalDisplay = 2;

            _mockCategoryRepository.Setup(x =>
                x.GetPagedCategoriesAsync(
                    query.PageIndex,
                    query.PageSize,
                    query.SortOrder,
                    query.SearchText,
                    query.BlogSiteId,
                    CancellationToken.None))
                .ReturnsAsync((categories, expectedTotal, expectedTotalDisplay))
                .Verifiable();

            //Act
            var result = await _handler.Handle(query, CancellationToken.None);

            //Assert
            this.ShouldSatisfyAllConditions(
                () => result.Item1.ShouldNotBeNull(),
                () => result.Item1.Count.ShouldBe(categories.Count),
                () => result.Item2.ShouldBe(expectedTotal),
                () => result.Item3.ShouldBe(expectedTotalDisplay),
                () => result.Item1[0].BlogSiteId.ShouldBe(blogSiteId),
                () => _mockCategoryRepository.VerifyAll());
        }
    }
}
