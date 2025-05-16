using CSharpFunctionalExtensions;
using MediatR;
using Shared.Models.Responses;
using User.Domain.Models.Requests;

namespace User.Application.Command
{
    public class PutUserCommand : IRequest<Result<ApiResponse>>
    {
        public PutUserCommand(UpdateUserRequest request)
        {
            Request = request;
        }

        public UpdateUserRequest Request { get; set; }
    }
}