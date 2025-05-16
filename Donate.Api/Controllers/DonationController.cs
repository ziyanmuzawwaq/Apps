using Donate.Application.Command;
using Donate.Application.Query;
using Donate.Domain.Models.Requests;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.Responses;
using System.ComponentModel.DataAnnotations;

namespace Donate.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DonationController : ValidationController<DonationController>
    {
        private readonly ILogger<DonationController> _logger;
        private readonly IMediator _mediator;

        public DonationController(
            ILogger<DonationController> logger,
            IMediator mediator,
            IEnumerable<IValidator> validators) : base(validators, logger)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("/api/donation/{donationId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ApiResponse>> GetDonationAsync([Required] int donationId)
        {
            try
            {
                _logger.LogInformation("Start GetDonationAsync");

                var query = new GetDonationQuery(donationId);
                var response = await ValidateAndExecute(query, (q) => _mediator.Send(query)).ConfigureAwait(false);

                _logger.LogInformation("End GetDonationAsync");

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetDonationAsync");
                throw;
            }
        }

        [HttpPost]
        [Route("/api/donation")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ApiResponse>> PostDonationAsync(CreateDonationRequest request)
        {
            try
            {
                _logger.LogInformation("Start PostDonationAsync");

                var query = new PostDonationCommand(request);
                var response = await ValidateAndExecute(query, (q) => _mediator.Send(query)).ConfigureAwait(false);

                _logger.LogInformation("End PostDonationAsync");

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in PostDonationAsync");
                throw;
            }
        }
    }
}