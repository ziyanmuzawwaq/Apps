using CSharpFunctionalExtensions;
using Donate.Domain.Models.Requests;
using MediatR;
using Shared.Models.Responses;

namespace Donate.Application.Command
{
    public class PostDonationCommand : IRequest<Result<ApiResponse>>
    {
        public PostDonationCommand(CreateDonationRequest request)
        {
            Request = request;
        }

        public CreateDonationRequest Request { get; set; }
    }
}