using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Shared.Models.Responses;
using System.Diagnostics.Contracts;
using User.Application.Helper.Implementation;
using User.Application.Interfaces;
using User.Domain.Entities.ViewModels;

namespace User.Application.Query
{
    public class GetDonationHistoryQueryHandler : IRequestHandler<GetDonationHistoryQuery, Result<ApiResponse>>
    {
        private readonly ILogger<GetDonationHistoryQueryHandler> _logger;
        private readonly IDonationService _donationService;
        private readonly IMemoryCache _memoryCache;

        public GetDonationHistoryQueryHandler(
            ILogger<GetDonationHistoryQueryHandler> logger,
            IDonationService donationService,
            IMemoryCache memoryCache)
        {
            _logger = logger;
            _donationService = donationService;
            _memoryCache = memoryCache;
        }

        public async Task<Result<ApiResponse>> Handle(GetDonationHistoryQuery request, CancellationToken cancellationToken)
        {
            Contract.Assert(request != null);

            _logger.LogTrace("Executing handler for request : {request}", nameof(GetDonationHistoryQuery));

            try
            {
                string cacheKey = string.Format($"{nameof(GetDonationHistoryQueryHandler)}-{request.UserId}");

                if (_memoryCache != null && _memoryCache.TryGetValue<List<DonationViewModel>>(cacheKey, out var cacheValue))
                {
                    return ResponseHelper.Success(cacheValue ?? new List<DonationViewModel>());
                }

                List<DonationViewModel>? donationLists = await _donationService.GetDonationByUserId(request.UserId);

                if (!donationLists.Any()) return ResponseHelper.Failed("There is no data found!");

                _memoryCache?.Set<List<DonationViewModel>>(cacheKey, donationLists ?? new List<DonationViewModel>(), TimeSpan.FromMinutes(5));

                return ResponseHelper.Success(donationLists ?? new List<DonationViewModel>());
            }
            catch (Exception ex)
            {
                return ResponseHelper.Failed(ex.Message);
            }
        }
    }
}