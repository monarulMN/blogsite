using Autofac.Extras.Moq;
using BlogSite.Application.Contracts;
using BlogSite.Application.Contracts.Repositories;
using BlogSite.Application.Features.Tags.Commands;
using BlogSite.Domain;
using BlogSite.Domain.Entities;
using MapsterMapper;
using Moq;
using NUnit.Framework;
using Shouldly;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Application.Tests.Features.Tags.Commands
{
    public class TagUpdateCommandHandlerTest
    {
        private AutoMock _mock;
        private TagUpdateCommandHandler _handler;
        private Mock<IApplicationUnitOfWork> _unitOfWorkMock;
        private Mock<ITagRepository> _tagRepositoryMock;
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
            _tagRepositoryMock = _mock.Mock<ITagRepository>();
            _mapperMock = _mock.Mock<IMapper>();

            _unitOfWorkMock.SetupGet(x => x.TagRepository)
                           .Returns(_tagRepositoryMock.Object);

            _handler = _mock.Create<TagUpdateCommandHandler>();
        }

        [TearDown]
        public void TearDown()
        {
            _unitOfWorkMock.Reset();
            _tagRepositoryMock.Reset();
            _mapperMock.Reset();
        }

        [Test]
        public async Task Handle_ValidCommand_UpdatesTag()
        {
            // Arrange
            var command = new TagUpdateCommand
            {
                Id = Guid.NewGuid(),
                Name = "Updated Tag",
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                UpdatedAt = DateTime.UtcNow
            };

            var mappedTag = new Tag
            {
                Id = command.Id,
                Name = command.Name,
                CreatedAt = command.CreatedAt,
                UpdatedAt = command.UpdatedAt
            };

            _mapperMock.Setup(m => m.Map<Tag>(command))
                       .Returns(mappedTag)
                       .Verifiable();

            _tagRepositoryMock.Setup(r => r.EditAsync(mappedTag, CancellationToken.None))
                              .Returns(Task.CompletedTask)
                              .Verifiable();

            _unitOfWorkMock.Setup(u => u.SaveAsync(CancellationToken.None))
                           .Returns(Task.CompletedTask)
                           .Verifiable();

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.ShouldBeOfType<Tag>();
            result.Id.ShouldBe(command.Id);
            result.Name.ShouldBe(command.Name);
            result.CreatedAt.ShouldBe(command.CreatedAt);
            result.UpdatedAt.ShouldBe(command.UpdatedAt);

            _mapperMock.VerifyAll();
            _tagRepositoryMock.VerifyAll();
            _unitOfWorkMock.VerifyAll();
        }
    }
}