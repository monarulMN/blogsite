using Autofac.Extras.Moq;
using BlogSite.Application.Contracts;
using BlogSite.Application.Contracts.Repositories;
using BlogSite.Application.Features.Tags.Commands;
using BlogSite.Domain;
using BlogSite.Domain.Entities;
using BlogSite.Domain.Enums;
using Moq;
using NUnit.Framework;
using Shouldly;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DevSkill.Application.Tests
{
    public class TagDeleteCommandHandlerTest
    {
        private AutoMock _mock;
        private TagDeleteCommandHandler _handler;
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

            _handler = _mock.Create<TagDeleteCommandHandler>();
        }

        [TearDown]
        public void TearDown()
        {
            _unitOfWorkMock.Reset();
            _tagRepositoryMock.Reset();
        }

        [Test]
        public async Task Handle_ValidCommand_MarksTagAsInactive()
        {
            // Arrange
            var tagId = Guid.NewGuid();
            var command = new TagDeleteCommand { Id = tagId };

            var existingTag = new Tag
            {
                Id = tagId,
                Name = "Sample Tag",
                Status = Status.Active
            };

            _tagRepositoryMock.Setup(r => r.GetByIdAsync(tagId, CancellationToken.None))
                              .ReturnsAsync(existingTag)
                              .Verifiable();

            _tagRepositoryMock.Setup(r => r.RemoveAsync(existingTag, CancellationToken.None))
                              .Returns(Task.CompletedTask)
                              .Verifiable();

            _unitOfWorkMock.Setup(u => u.SaveAsync(CancellationToken.None))
                           .Returns(Task.CompletedTask)
                           .Verifiable();

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            this.ShouldSatisfyAllConditions(
                () => result.ShouldBe(tagId),
                () => existingTag.Status.ShouldBe(Status.Inactive),
                () => _tagRepositoryMock.VerifyAll(),
                () => _unitOfWorkMock.VerifyAll()
            );
            
        }
    }
}