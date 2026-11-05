using Autofac.Extras.Moq;
using BlogSite.Application.Contracts;
using BlogSite.Application.Contracts.Repositories;
using BlogSite.Application.Features.Reports.Queries;
using BlogSite.Domain;
using BlogSite.Domain.Entities;
using Moq;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Application.Tests.Features.Reports.Queries
{
    public class GetReportsAdminQueryHandlerTest
    {
        private AutoMock _mock;
        private GetReportsAdminQueryHandler _handler;
        private Mock<IApplicationUnitOfWork> _unitOfWorkMock;
        private Mock<IReportRepository> _reportRepositoryMock;

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
            _reportRepositoryMock = _mock.Mock<IReportRepository>();

            _unitOfWorkMock.SetupGet(x => x.ReportRepository)
                           .Returns(_reportRepositoryMock.Object);

            _handler = _mock.Create<GetReportsAdminQueryHandler>();
        }

        [TearDown]
        public void TearDown()
        {
            _unitOfWorkMock.Reset();
            _reportRepositoryMock.Reset();
        }

        [Test]
        public async Task Handle_ValidQuery_ReturnsPagedReports()
        {
            // Arrange
            var query = new GetReportsAdminQuery
            {
                PageSize = 10,
                PageIndex = 1,
                SearchText = "test",
                SortOrder = "asc"
            };

            var reports = new List<Report>
            {
                new Report { Id = Guid.NewGuid(), ReportText = "Report 1" },
                new Report { Id = Guid.NewGuid(), ReportText = "Report 2" },
            };

            var expectedTotal = reports.Count;
            var expectedDisplayTotal = reports.Count;

            _reportRepositoryMock
                .Setup(r => r.GetPagedAdminReportsAsync(
                    query.PageSize, 
                    query.PageIndex, 
                    query.SortOrder, 
                    query.SearchText,
                    CancellationToken.None))
                .ReturnsAsync((reports, expectedTotal, expectedDisplayTotal))
                .Verifiable();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            this.ShouldSatisfyAllConditions(
                () => result.Item1.ShouldBe(reports),
                () => result.Item2.ShouldBe(expectedTotal),
                () => result.Item3.ShouldBe(expectedDisplayTotal),
                () => _reportRepositoryMock.VerifyAll()
            );
        }
    }
}