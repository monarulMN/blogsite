using BlogSite.Application.Contracts;
using BlogSite.Application.Contracts.Repositories;
using BlogSite.Application.Features.Users.Queries;
using BlogSite.Domain.Entities;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Application.Tests.Features.Users
{
    public class GetAllBlockedUsersQueryHandlerTest
    {
        private Mock<IApplicationUnitOfWork> _unitOfWorkMock;
        private Mock<IUserRepository> _userRepositoryMock;
        private GetAllBlockedUsersQueryHandler _handler;


        [SetUp]
        public void Setup()
        {
            _unitOfWorkMock = new Mock<IApplicationUnitOfWork>();
            _userRepositoryMock = new Mock<IUserRepository>();

            _unitOfWorkMock.Setup(u => u.UserRepository)
                .Returns(_userRepositoryMock.Object);

            _handler = new GetAllBlockedUsersQueryHandler(_unitOfWorkMock.Object);
        }


        [Test]
        public async Task Handle_ReturnsBlockedUsers_WithPagination()
        {
            // Arrange
            var users = new List<User>
            {
                new User
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Alice",
                    LastName = "Brown",
                    UserName = "aliceb",
                    Email = "alice.brown@example.com"
                },

                new User
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Bob",
                    LastName = "Smith",
                    UserName = "bobsmith",
                    Email = "bob.smith@example.com"
                }
            };

            var total = users.Count();
            var totalDisplay = users.Count();

            _userRepositoryMock
                .Setup(r => r.GetPagedBlockedUsersAsync(10, 1, "asc", null, CancellationToken.None))
                .ReturnsAsync((users, total, totalDisplay));

            var query = new GetAllBlockedUsersQuery
            {
                PageSize = 10,
                PageIndex = 1,
                SortOrder = "asc",
                SearchText = null
            };

            //Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.data.ShouldNotBeNull();
            result.data.Count.ShouldBe(2);
            result.total.ShouldBe(total);
            result.totalDisplay.ShouldBe(totalDisplay);

            result.data[0].FirstName.ShouldBe("Alice");
            result.data[1].FirstName.ShouldBe("Bob");

            _userRepositoryMock.Verify(r =>
                r.GetPagedBlockedUsersAsync(10, 1, "asc", null, CancellationToken.None), Times.Once);
        }
    }
}
