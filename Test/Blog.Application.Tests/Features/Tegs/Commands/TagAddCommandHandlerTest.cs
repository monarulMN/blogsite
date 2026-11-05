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
    public class TagAddCommandHandlerTest
    {
        private AutoMock _mock;
        private TagAddCommandHandler _handler;
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

            _handler = _mock.Create<TagAddCommandHandler>();
        }

        [TearDown]
        public void TearDown()
        {
            _unitOfWorkMock.Reset();
            _tagRepositoryMock.Reset();
            _mapperMock.Reset();
        }

        [Test]
        public async Task Handle_ValidCommand_AddsTag()
        {
            // Arrange
            var command = new TagAddCommand
            {
                Name = "New Tag",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var mappedTag = new Tag
            {
                Name = command.Name,
                CreatedAt = command.CreatedAt,
                UpdatedAt = command.UpdatedAt
            };

            _mapperMock.Setup(m => m.Map<Tag>(command))
                       .Returns(mappedTag)
                       .Verifiable();

            _tagRepositoryMock.Setup(r => r.AddAsync(mappedTag, CancellationToken.None))
                              .Returns(Task.CompletedTask)
                              .Verifiable();

            _unitOfWorkMock.Setup(u => u.SaveAsync(CancellationToken.None))
                           .Returns(Task.CompletedTask)
                           .Verifiable();

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            this.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.ShouldBeOfType<Tag>(),
                () => result.Name.ShouldBe(command.Name),
                () => result.CreatedAt.ShouldBe(command.CreatedAt),
                () => result.UpdatedAt.ShouldBe(command.UpdatedAt),
                () => result.Id.ShouldNotBe(Guid.Empty), // Ensure IdentityGenerator assigned an Id
                () => _mapperMock.VerifyAll(),
                () => _tagRepositoryMock.VerifyAll(),
                () => _unitOfWorkMock.VerifyAll()
            );
        }
    }
}