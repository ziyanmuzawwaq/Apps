using Donate.Application.Services;
using Donate.Domain.Entities;
using Donate.Domain.Interfaces;
using Moq;

namespace Donate.Tests;

public class DonationServiceTests
{
    [Fact]
    public async Task GetDonationList_ReturnsCorrectList()
    {
        var donationId = 1;
        var mockRepo = new Mock<IDonationRepository>();

        mockRepo.Setup(r => r.GetById(donationId, default))
                .ReturnsAsync(new Donation { DonationId = donationId, Amount = 50000 });

        var service = new DonationService(mockRepo.Object);

        var result = await service.GetDonationById(donationId);

        Assert.Equal(50000, result!.Amount);
    }

    [Fact]
    public async Task GetDonationList_DonationNotFound_ReturnsNotFound()
    {
        var donationId = 0;
        var mockRepo = new Mock<IDonationRepository>();

        mockRepo.Setup(r => r.GetById(donationId, default))
                .ReturnsAsync((Donation?)null);

        var service = new DonationService(mockRepo.Object);

        var result = await service.GetDonationById(donationId);

        Assert.Null(result);
    }
}