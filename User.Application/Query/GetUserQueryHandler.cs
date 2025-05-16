using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Shared.Models.Responses;
using System.Diagnostics.Contracts;
using User.Application.Helper.Implementation;
using User.Application.Interfaces;
using User.Domain.Entities;

namespace User.Application.Query
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, Result<ApiResponse>>
    {
        private readonly ILogger<GetUserQueryHandler> _logger;
        private readonly IUserService _userService;
        private readonly IMemoryCache _memoryCache;

        public GetUserQueryHandler(
            ILogger<GetUserQueryHandler> logger,
            IUserService userService,
            IMemoryCache memoryCache)
        {
            _logger = logger;
            _userService = userService;
            _memoryCache = memoryCache;
        }

        public async Task<Result<ApiResponse>> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            Contract.Assert(request != null);

            _logger.LogTrace("Executing handler for request : {request}", nameof(GetUserQuery));

            try
            {
                string cacheKey = string.Format($"{nameof(GetUserQueryHandler)}-{request.UserId}");

                if (request.UserId is not null)
                {
                    if (_memoryCache != null && _memoryCache.TryGetValue<Users>(cacheKey, out var cacheValue))
                    {
                        return ResponseHelper.Success(cacheValue ?? new Users());
                    }

                    var userData = await _userService.GetUserByIdAsync((int)request.UserId, cancellationToken);

                    if (userData is null) return ResponseHelper.Failed("There is no data found!");

                    _memoryCache?.Set<Users>(cacheKey, userData ?? new Users(), TimeSpan.FromMinutes(5));

                    return ResponseHelper.Success(userData ?? new Users());
                }
                else
                {
                    if (_memoryCache != null && _memoryCache.TryGetValue<List<Users>>(cacheKey, out var cacheValue))
                    {
                        return ResponseHelper.Success(cacheValue ?? new List<Users>());
                    }

                    var userData = await _userService.GetAllUserAsync();

                    if (userData is null || !userData.Any()) return ResponseHelper.Failed("There is no data found!");

                    _memoryCache?.Set<List<Users>>(cacheKey, userData ?? new List<Users>(), TimeSpan.FromMinutes(5));

                    return ResponseHelper.Success(userData ?? new List<Users>());
                }
            }
            catch (Exception ex)
            {
                return ResponseHelper.Failed(ex.Message);
            }
        }
    }
}