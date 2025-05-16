using CSharpFunctionalExtensions;
using MediatR;
using Shared.Models.Responses;

namespace User.Application.Query
{
    public class GetDonationHistoryQuery : IRequest<Result<ApiResponse>>
    {
        public GetDonationHistoryQuery(int userId)
        {
            UserId = userId;
        }

        public int UserId { get; set; }
    }
}