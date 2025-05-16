using Moq;
using User.Application.Services;
using User.Domain.Interfaces;

namespace User.Tests;

public class DonationServiceTests
{
    [Fact]
    public async Task GetDonationList_ReturnsCorrectList()
    {
        var userId = 1;
        var mockRepo = new Mock<IDonationRepository>();

        var service = new DonationService(mockRepo.Object);

        var result = await Task.Run(() => service.GetDonationByUserId(userId));

        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetDonationList_DonationNotFound_ReturnsNotFound()
    {
        var userId = 123;
        var mockRepo = new Mock<IDonationRepository>();

        var service = new DonationService(mockRepo.Object);

        var result = await Task.Run(() => service.GetDonationByUserId(userId));

        Assert.Empty(result);
    }
}