using Donate.Application.Services;
using Donate.Domain.Interfaces;
using Moq;
using Shared.Entities;

namespace Donate.Tests
{
    public class EventJobServiceTests
    {
        [Fact]
        public async Task GetEventJobName_ReturnsCorrectName()
        {
            string name = "EventConsumerJob";
            var mockRepo = new Mock<IEventJobRepository>();

            mockRepo.Setup(r => r.GetByName(name, default))
                    .ReturnsAsync(new EventJobMonitoring { EventJobName = name, Status = "Online" });

            var service = new EventJobService(mockRepo.Object);

            var result = await service.GetEventJobMonitoringByName(name);

            Assert.Equal("Online", result!.Status);
        }

        [Fact]
        public async Task GetEventJobName_EventJobNotFound_ReturnsNotFound()
        {
            string name = "EventConsumerJob";
            var mockRepo = new Mock<IEventJobRepository>();

            mockRepo.Setup(r => r.GetByName(name, default))
                    .ReturnsAsync((EventJobMonitoring?)null);

            var service = new EventJobService(mockRepo.Object);

            var result = await service.GetEventJobMonitoringByName(name);

            Assert.Null(result);
        }
    }
}