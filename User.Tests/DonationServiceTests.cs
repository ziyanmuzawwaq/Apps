using Moq;
using User.Application.Services;
using User.Domain.Entities.ViewModels;
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

        mockRepo.Setup(r => r.GetByUserId(userId))
                .ReturnsAsync(new List<DonationViewModel> { new DonationViewModel { UserId = userId, Amount = 5000 } });

        var result = await service.GetDonationByUserId(userId);

        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task GetDonationList_DonationNotFound_ReturnsNotFound()
    {
        var userId = 123;
        var mockRepo = new Mock<IDonationRepository>();

        mockRepo.Setup(r => r.GetByUserId(userId))
                .ReturnsAsync(new List<DonationViewModel>());

        var service = new DonationService(mockRepo.Object);

        var result = await service.GetDonationByUserId(userId);

        Assert.Empty(result);
    }
}