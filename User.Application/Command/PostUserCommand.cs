using CSharpFunctionalExtensions;
using MediatR;
using Shared.Models.Responses;
using User.Domain.Models.Requests;

namespace User.Application.Command
{
    public class PostUserCommand : IRequest<Result<ApiResponse>>
    {
        public PostUserCommand(CreateUserRequest request)
        {
            Request = request;
        }

        public CreateUserRequest Request { get; set; }
    }
}