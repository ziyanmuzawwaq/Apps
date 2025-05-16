using CSharpFunctionalExtensions;
using MediatR;
using Shared.Models.Responses;

namespace User.Application.Query
{
    public class GetUserQuery : IRequest<Result<ApiResponse>>
    {
        public GetUserQuery(int? userId)
        {
            UserId = userId;
        }

        public int? UserId { get; set; }
    }
}