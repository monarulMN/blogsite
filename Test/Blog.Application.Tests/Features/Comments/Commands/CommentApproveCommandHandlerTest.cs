using Autofac.Extras.Moq;
using BlogSite.Application.Contracts.Repositories;
using BlogSite.Application.Features.Comments.Commands;
using BlogSite.Domain;
using BlogSite.Domain.Entities;
using BlogSite.Domain.Enums;
using Moq;
using NUnit.Framework;
using Shouldly;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DevSkill.Application.Tests;

public class CommentApproveCommandHandlerTest
{
    private AutoMock _mock;
    private CommentApproveCommandHandler _handler;
    private Mock<IApplicationUnitOfWork> _unitOfWorkMock;
    private Mock<ICommentRepository> _commentRepositoryMock;

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

        _unitOfWorkMock.SetupGet(x => x.CommentRepository)
            .Returns(_commentRepositoryMock.Object)
            .Verifiable();

        _handler = _mock.Create<CommentApproveCommandHandler>();
    }

    [TearDown]
    public void TearDown()
    {
        _unitOfWorkMock.Reset();
        _commentRepositoryMock.Reset();
    }

    [Test]
    public async Task Handle_ValidCommand_ApprovesComment()
    {
        // Arrange
        var commentId = Guid.NewGuid();
        var command = new CommentApproveCommand
        {
            CommentId = commentId
        };

        var existingComment = new Comment
        {
            Id = commentId,
            Status = Status.Inactive
        };

        // Mock GetByIdAsync
        _commentRepositoryMock
            .Setup(r => r.GetByIdAsync(commentId, CancellationToken.None))
            .ReturnsAsync(existingComment)
            .Verifiable();

        // Mock EditAsync
        _commentRepositoryMock
            .Setup(r => r.EditAsync(existingComment, CancellationToken.None))
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
            () => result.ShouldBe(commentId),
            () => existingComment.Status.ShouldBe(Status.Active),
            () => _commentRepositoryMock.VerifyAll(),
            () => _unitOfWorkMock.VerifyAll()
        );
    }
}