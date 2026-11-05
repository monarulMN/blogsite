using Autofac.Extras.Moq;
using BlogSite.Application.Contracts;
using BlogSite.Application.Contracts.Repositories;
using BlogSite.Application.Features.Tags.Queries;
using BlogSite.Domain;
using BlogSite.Domain.Entities;
using Moq;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Application.Tests.Features.Tags.Queries
{
    public class GetTagsQueryHandlerTest
    {
        private AutoMock _mock;
        private GetTagsQueryHandler _handler;
        private Mock<IApplicationUnitOfWork> _unitOfWorkMock;
        private Mock<ITagRepository> _tagRepositoryMock;

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
            _tagRepositoryMock = _mock.Mock<ITagRepository>();

            _unitOfWorkMock.SetupGet(x => x.TagRepository)
                           .Returns(_tagRepositoryMock.Object);

            _handler = _mock.Create<GetTagsQueryHandler>();
        }

        [TearDown]
        public void TearDown()
        {
            _unitOfWorkMock.Reset();
            _tagRepositoryMock.Reset();
        }

        [Test]
        public async Task Handle_ValidQuery_ReturnsPagedTags()
        {
            // Arrange
            var query = new GetTagsQuery
            {
                PageSize = 5,
                PageIndex = 1,
                SearchText = "Test",
                SortOrder = "Name"
            };

            var expectedData = new List<Tag>
            {
                new Tag { Id = Guid.NewGuid(), Name = "Tag1" },
                new Tag { Id = Guid.NewGuid(), Name = "Tag2" }
            };

            _tagRepositoryMock.Setup(r => r.GetPagedTagsAsync(query,
                CancellationToken.None))
                .ReturnsAsync((expectedData, expectedData.Count, expectedData.Count))
                .Verifiable();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            this.ShouldSatisfyAllConditions(
                () => result.Item1.ShouldBe(expectedData),
                () => result.Item2.ShouldBe(expectedData.Count),
                () => result.Item3.ShouldBe(expectedData.Count),
                () => _tagRepositoryMock.VerifyAll()
            );
        }
    }
}