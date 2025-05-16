using Donate.Application.Services;
using Donate.Domain.Entities.ViewModels;
using Donate.Domain.Interfaces;
using Moq;

namespace Donate.Tests
{
    public class UserServiceTests
    {
        [Fact]
        public async Task GetUserName_ReturnsCorrectName()
        {
            var userId = 1;
            var mockRepo = new Mock<IUserRepository>();

            mockRepo.Setup(r => r.GetById(userId, default))
                    .ReturnsAsync(new UserViewModel { UserId = userId, Name = "Ziyan" });

            var service = new UserService(mockRepo.Object);

            var result = await service.GetUserById(userId);

            Assert.Equal("Ziyan", result!.Name);
        }

        [Fact]
        public async Task GetUserName_UserNotFound_ReturnsNotFound()
        {
            var userId = 0;
            var mockRepo = new Mock<IUserRepository>();

            mockRepo.Setup(r => r.GetById(userId, default))
                    .ReturnsAsync((UserViewModel?)null);

            var service = new UserService(mockRepo.Object);

            var result = await service.GetUserById(userId);

            Assert.Null(result);
        }
    }
}