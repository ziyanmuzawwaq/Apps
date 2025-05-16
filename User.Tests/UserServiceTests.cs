using Moq;
using User.Application.Services;
using User.Domain.Entities;
using User.Domain.Interfaces;

namespace User.Tests
{
    public class UserServiceTests
    {
        [Fact]
        public async Task GetUserName_ReturnsCorrectName()
        {
            var userId = 2;
            var mockRepo = new Mock<IUserRepository>();

            mockRepo.Setup(r => r.GetById(userId))
                    .ReturnsAsync(new Users { UserId = userId, Name = "Ziyan" });

            var service = new UserService(mockRepo.Object);

            var result = await service.GetUserByIdAsync(userId);

            Assert.Equal("Ziyan", result!.Name);
        }

        [Fact]
        public async Task GetUserName_UserNotFound_ReturnsNotFound()
        {
            var userId = 123;
            var mockRepo = new Mock<IUserRepository>();

            mockRepo.Setup(r => r.GetById(userId))
                    .ReturnsAsync((Users?)null);

            var service = new UserService(mockRepo.Object);

            var result = await service.GetUserByIdAsync(userId);

            Assert.Null(result);
        }
    }
}