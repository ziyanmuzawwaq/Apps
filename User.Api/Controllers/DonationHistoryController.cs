using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.Responses;
using System.ComponentModel.DataAnnotations;
using User.Application.Query;

namespace User.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DonationHistoryController : ValidationController<DonationHistoryController>
    {
        private readonly ILogger<DonationHistoryController> _logger;
        private readonly IMediator _mediator;

        public DonationHistoryController(
            ILogger<DonationHistoryController> logger,
            IMediator mediator,
            IEnumerable<IValidator> validators) : base(validators, logger)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("/api/donation/history/{userId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ApiResponse>> GetDonationHistoryAsync([Required] int userId)
        {
            try
            {
                _logger.LogInformation("Start GetDonationHistoryAsync");

                var query = new GetDonationHistoryQuery(userId);
                var response = await ValidateAndExecute(query, (q) => _mediator.Send(query)).ConfigureAwait(false);

                _logger.LogInformation("End GetDonationHistoryAsync");

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetDonationHistoryAsync");
                throw;
            }
        }
    }
}