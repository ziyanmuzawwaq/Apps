using CSharpFunctionalExtensions;
using Donate.Application.Helper.Implementation;
using Donate.Application.Interfaces;
using Donate.Domain.Entities;
using Donate.Domain.Interfaces.Messaging;
using Donate.Domain.Models.Requests;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Models.Responses;
using System.Diagnostics.Contracts;
using System.Text.Json;

namespace Donate.Application.Command
{
    public class PostDonationCommandHandler : IRequestHandler<PostDonationCommand, Result<ApiResponse>>
    {
        private readonly ILogger<PostDonationCommandHandler> _logger;
        private readonly IDonationService _donationService;
        private readonly IValidator<CreateDonationRequest> _validator;
        private readonly IMessageProducer _messageProducer;

        public PostDonationCommandHandler(
            ILogger<PostDonationCommandHandler> logger,
            IDonationService donationService,
            IValidator<CreateDonationRequest> validator,
            IMessageProducer messageProducer)
        {
            _logger = logger;
            _donationService = donationService;
            _validator = validator;
            _messageProducer = messageProducer;
        }

        public async Task<Result<ApiResponse>> Handle(PostDonationCommand request, CancellationToken cancellationToken)
        {
            Contract.Assert(request != null);

            _logger.LogTrace("Executing handler for request : {request}", nameof(PostDonationCommand));

            try
            {
                ValidationResult validationResult = await _validator.ValidateAsync(request.Request);

                if (!validationResult.IsValid)
                {
                    return ResponseHelper.Failed("Error validation", validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));
                }

                Donation donation = new();
                donation.UserId = request.Request.UserId;
                donation.DonorName = request.Request.DonorName;
                donation.DonorEmail = request.Request.DonorEmail;
                donation.Amount = request.Request.Amount;

                var result = await _donationService.CreateDonation(donation, cancellationToken).ConfigureAwait(false);

                if (result == null) return ResponseHelper.Failed("Create donation data failed!");

                // Publish to Kafka
                var json = JsonSerializer.Serialize(result);
                await _messageProducer.PublishAsync("donation-created", json, cancellationToken);

                return ResponseHelper.Success(donation);
            }
            catch (Exception ex)
            {
                return ResponseHelper.Failed(ex.Message);
            }
        }
    }
}