using Autofac.Extras.Moq;
using BlogSite.Application.Contracts;
using BlogSite.Application.Contracts.Repositories;
using BlogSite.Application.Features.Reports.Commands;
using BlogSite.Domain;
using BlogSite.Domain.Entities;
using BlogSite.Domain.Enums;
using Moq;
using NUnit.Framework;
using Shouldly;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Application.Tests.Features.Reports.Commands
{
    public class ReportDeleteCommandHandlerTest
    {
        private AutoMock _mock;
        private ReportDeleteCommandHandler _handler;
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

            _handler = _mock.Create<ReportDeleteCommandHandler>();
        }

        [TearDown]
        public void TearDown()
        {
            _unitOfWorkMock.Reset();
            _reportRepositoryMock.Reset();
        }

        [Test]
        public async Task Handle_ValidCommand_DeletesReport()
        {
            // Arrange
            var reportId = Guid.NewGuid();
            var command = new ReportDeleteCommand { Id = reportId };

            var existingReport = new Report
            {
                Id = reportId,
                Status = Status.Active
            };

            // Mock GetByIdAsync
            _reportRepositoryMock
                .Setup(r => r.GetByIdAsync(reportId, CancellationToken.None))
                .ReturnsAsync(existingReport)
                .Verifiable();

            // Mock RemoveAsync
            _reportRepositoryMock
                .Setup(r => r.RemoveAsync(existingReport, CancellationToken.None))
                .Returns(Task.CompletedTask)
                .Verifiable();

            // Mock SaveAsync
            _unitOfWorkMock
                .Setup(u => u.SaveAsync(CancellationToken.None))
                .Returns(Task.CompletedTask)
                .Verifiable();

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            this.ShouldSatisfyAllConditions(
                () => result.ShouldBe(reportId),
                () => existingReport.Status.ShouldBe(Status.Inactive),
                () => _reportRepositoryMock.VerifyAll(),
                () => _unitOfWorkMock.VerifyAll()
            );
        }
    }
}