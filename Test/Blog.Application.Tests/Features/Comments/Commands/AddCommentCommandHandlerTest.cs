using Autofac.Extras.Moq;
using BlogSite.Application.Contracts;
using BlogSite.Application.Contracts.Repositories;
using BlogSite.Application.Features.Comments.Commands;
using BlogSite.Domain;
using BlogSite.Domain.Entities;
using MapsterMapper;
using Moq;
using NUnit.Framework;
using Shouldly;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Application.Tests;

public class AddCommentCommandHandlerTest
{
    private AutoMock _mock;
    private AddCommentCommandHandler _handler;
    private Mock<IApplicationUnitOfWork> _unitOfWorkMock;
    private Mock<ICommentRepository> _commentRepositoryMock;
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
        _commentRepositoryMock = _mock.Mock<ICommentRepository>();
        _mapperMock = _mock.Mock<IMapper>();

        _unitOfWorkMock.SetupGet(x => x.CommentRepository)
            .Returns(_commentRepositoryMock.Object);

        _handler = _mock.Create<AddCommentCommandHandler>();
    }

    [TearDown]
    public void TearDown()
    {
        _unitOfWorkMock.Reset();
        _commentRepositoryMock.Reset();
        _mapperMock.Reset();
    }

    [Test]
    public async Task Handle_ValidCommand_AddsAndReturnsComment()
    {
        // Arrange
        var command = new AddCommentCommand
        {
            Name = "John Doe",
            Email = "john@example.com",
            Text = "This is a comment",
            BlogPostId = Guid.NewGuid(),
            BlogSiteId = Guid.NewGuid(),
            ParentCommentId = null,
            CreateAt = DateTime.UtcNow
        };

        var mappedComment = new Comment
        {
            Name = command.Name,
            Email = command.Email,
            Text = command.Text,
            BlogPostId = command.BlogPostId,
            BlogsiteId = command.BlogSiteId,
            ParentCommentId = command.ParentCommentId,
            CreateAt = command.CreateAt
        };

        _mapperMock.Setup(m => m.Map<Comment>(command))
            .Returns(mappedComment)
            .Verifiable();

        _commentRepositoryMock.Setup(r => r.AddAsync(mappedComment, CancellationToken.None))
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
            () => result.ShouldBeOfType<Comment>(),
            () => result.Name.ShouldBe(command.Name),
            () => result.Email.ShouldBe(command.Email),
            () => result.Text.ShouldBe(command.Text),
            () => result.BlogPostId.ShouldBe(command.BlogPostId),
            () => result.BlogsiteId.ShouldBe(command.BlogSiteId),
            () => result.ParentCommentId.ShouldBe(command.ParentCommentId),
            () => _mapperMock.VerifyAll(),
            () => _commentRepositoryMock.VerifyAll(),
            () => _unitOfWorkMock.VerifyAll()
        );
    }
}