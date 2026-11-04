using Autofac.Extras.Moq;
using BlogSite.Application.Contracts;
using BlogSite.Application.Contracts.Repositories;
using BlogSite.Application.Features.Reports.Commands;
using BlogSite.Domain;
using BlogSite.Domain.Entities;
using MapsterMapper;
using Moq;
using NUnit.Framework;
using Shouldly;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DevSkill.Application.Tests
{
    public class AddReportCommandHandlerTest
    {
        private AutoMock _mock;
        private AddReportCommandHandler _handler;
        private Mock<IApplicationUnitOfWork> _unitOfWorkMock;
        private Mock<IReportRepository> _reportRepositoryMock;
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
            _reportRepositoryMock = _mock.Mock<IReportRepository>();
            _mapperMock = _mock.Mock<IMapper>();

            _unitOfWorkMock.SetupGet(x => x.ReportRepository)
                           .Returns(_reportRepositoryMock.Object);

            _handler = _mock.Create<AddReportCommandHandler>();
        }

        [TearDown]
        public void TearDown()
        {
            _unitOfWorkMock.Reset();
            _reportRepositoryMock.Reset();
            _mapperMock.Reset();
        }

        [Test]
        public async Task Handle_ValidCommand_AddsReport()
        {
            // Arrange
            var blogPostId = Guid.NewGuid();
            var command = new AddReportCommand
            {
                BlogPostId = blogPostId,
                ReportText = "Inappropriate content",
                CreatedAt = DateTime.UtcNow
            };

            var mappedReport = new Report
            {
                BlogPostId = command.BlogPostId,
                ReportText = command.ReportText,
                CreatedAt = command.CreatedAt
            };

            // Mock mapper
            _mapperMock
                .Setup(m => m.Map<Report>(command))
                .Returns(mappedReport)
                .Verifiable();

            // Mock AddAsync
            _reportRepositoryMock
                .Setup(r => r.AddAsync(mappedReport, CancellationToken.None))
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
                () => result.ShouldNotBeNull(),
                () => result.ReportText.ShouldBe(command.ReportText),
                () => result.BlogPostId.ShouldBe(command.BlogPostId),
                () => _mapperMock.VerifyAll(),
                () => _reportRepositoryMock.VerifyAll(),
                () => _unitOfWorkMock.VerifyAll()
            );
        }
    }
}