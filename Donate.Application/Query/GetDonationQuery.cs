using CSharpFunctionalExtensions;
using MediatR;
using Shared.Models.Responses;

namespace Donate.Application.Query
{
    public class GetDonationQuery : IRequest<Result<ApiResponse>>
    {
        public GetDonationQuery(int donationId)
        {
            DonationId = donationId;
        }

        public int DonationId { get; set; }
    }
}