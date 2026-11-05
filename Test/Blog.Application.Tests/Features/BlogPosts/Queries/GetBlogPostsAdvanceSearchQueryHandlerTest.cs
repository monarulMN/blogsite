using Autofac.Extras.Moq;
using BlogSite.Application.Contracts;
using BlogSite.Application.Features.BlogPosts.Queries;
using BlogSite.Domain;
using BlogSite.Domain.Dtos;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace Blog.Application.Tests.Features.BlogPosts.Queries
{

    public class GetBlogPostsAdvanceSearchQueryHandlerTest
    {
        private AutoMock _mock;
        private GetBlogPostsAdvanceSearchQueryHandler _handler;
        private Mock<IApplicationUnitOfWork> _unitOfWorkMock;
        private Mock<ISqlUtility> _sqlUtilityMock;

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
            _sqlUtilityMock = _mock.Mock<ISqlUtility>();

            _handler = _mock.Create<GetBlogPostsAdvanceSearchQueryHandler>();
        }

        [TearDown]
        public void TearDown()
        {
            _unitOfWorkMock.Reset();
            _sqlUtilityMock.Reset();
        }

        [Test]
        public async Task Handle_ValidQuery_ReturnsStoredProcedureResult()
        {
            // Arrange
            var query = new GetBlogPostsAdvanceSearchQuery
            {
                PageIndex = 1,
                PageSize = 10,
                SortOrder = "Title ASC",
                Title = "Test",
                DateFrom = DateTime.UtcNow.AddDays(-10),
                DateTo = DateTime.UtcNow
            };

            var expectedData = new List<BlogPostDto>
        {
            new BlogPostDto { Title = "Post 1" },
            new BlogPostDto { Title = "Post 2" }
        };

            var outValues = new Dictionary<string, object>
        {
            { "Total", 50 },
            { "TotalDisplay", 2 }
        };

            var expectedOutput = (result: (IList<BlogPostDto>)expectedData, outValues: outValues);

            _sqlUtilityMock
                .Setup(x => x.QueryWithStoredProcedureAsync<BlogPostDto>(
                    "GetBlogPostSp",
                    It.Is<IDictionary<string, object?>>(p =>
                        (int)p["PageIndex"] == query.PageIndex &&
                        (int)p["PageSize"] == query.PageSize &&
                        (string)p["OrderBy"] == query.SortOrder &&
                        (string)p["Title"] == query.Title &&
                        (DateTime)p["DateFrom"] == query.DateFrom &&
                        (DateTime)p["DateTo"] == query.DateTo
                    ),
                    It.IsAny<Dictionary<string, Type>>()))
                .ReturnsAsync(expectedOutput)
                .Verifiable();

            _unitOfWorkMock
                .SetupGet(x => x.SqlUtility)
                .Returns(_sqlUtilityMock.Object)
                .Verifiable();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert

            this.ShouldSatisfyAllConditions(
                () => result.Item1.ShouldNotBeNull(),
                () => result.Item1.Count.ShouldBe(expectedData.Count),
                () => result.Item2.ShouldBe(50),
                () => result.Item3.ShouldBe(2),
                () => _unitOfWorkMock.VerifyAll(),
                () => _sqlUtilityMock.VerifyAll()
            );
        }
    }
}