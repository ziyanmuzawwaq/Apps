using CSharpFunctionalExtensions;
using Donate.Application.Helper.Implementation;
using Donate.Application.Interfaces;
using Donate.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Shared.Models.Responses;
using System.Diagnostics.Contracts;

namespace Donate.Application.Query
{
    public class GetDonationQueryHandler : IRequestHandler<GetDonationQuery, Result<ApiResponse>>
    {
        private readonly ILogger<GetDonationQueryHandler> _logger;
        private readonly IDonationService _donationService;
        private readonly IMemoryCache _memoryCache;

        public GetDonationQueryHandler(
            ILogger<GetDonationQueryHandler> logger,
            IDonationService donationService,
            IMemoryCache memoryCache)
        {
            _logger = logger;
            _donationService = donationService;
            _memoryCache = memoryCache;
        }

        public async Task<Result<ApiResponse>> Handle(GetDonationQuery request, CancellationToken cancellationToken)
        {
            Contract.Assert(request != null);

            _logger.LogTrace("Executing handler for request : {request}", nameof(GetDonationQuery));

            try
            {
                string cacheKey = string.Format($"{nameof(GetDonationQueryHandler)}-{request.DonationId}");

                if (_memoryCache != null && _memoryCache.TryGetValue<Donation>(cacheKey, out var cacheValue))
                {
                    return ResponseHelper.Success(cacheValue ?? new Donation());
                }

                var donationData = await _donationService.GetDonationById(request.DonationId, cancellationToken);

                if (donationData == null) return ResponseHelper.Failed("There is no data found!");

                _memoryCache?.Set<Donation>(cacheKey, donationData ?? new Donation(), TimeSpan.FromMinutes(5));

                return ResponseHelper.Success(donationData ?? new Donation());
            }
            catch (Exception ex)
            {
                return ResponseHelper.Failed(ex.Message);
            }
        }
    }
}