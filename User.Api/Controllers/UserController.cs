using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.Responses;
using User.Application.Command;
using User.Application.Query;
using User.Domain.Models.Requests;

namespace User.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ValidationController<UserController>
    {
        private readonly ILogger<UserController> _logger;
        private readonly IMediator _mediator;

        public UserController(
            ILogger<UserController> logger,
            IMediator mediator,
            IEnumerable<IValidator> validators) : base(validators, logger)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("/api/user")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ApiResponse>> GetUserAsync(int? userId)
        {
            try
            {
                _logger.LogInformation("Start GetUserAsync");

                var query = new GetUserQuery(userId);
                var response = await ValidateAndExecute(query, (q) => _mediator.Send(query)).ConfigureAwait(false);

                _logger.LogInformation("End GetUserAsync");

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetUserAsync");
                throw;
            }
        }

        [HttpPost]
        [Route("/api/user")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ApiResponse>> PostUserAsync(CreateUserRequest request)
        {
            try
            {
                _logger.LogInformation("Start PostUserAsync");

                var query = new PostUserCommand(request);
                var response = await ValidateAndExecute(query, (q) => _mediator.Send(query)).ConfigureAwait(false);

                _logger.LogInformation("End PostUserAsync");

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in PostUserAsync");
                throw;
            }
        }

        [HttpPut]
        [Route("/api/user")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ApiResponse>> PutUserAsync(UpdateUserRequest request)
        {
            try
            {
                _logger.LogInformation("Start PutUserAsync");

                var query = new PutUserCommand(request);
                var response = await ValidateAndExecute(query, (q) => _mediator.Send(query)).ConfigureAwait(false);

                _logger.LogInformation("End PutUserAsync");

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in PutUserAsync");
                throw;
            }
        }
    }
}