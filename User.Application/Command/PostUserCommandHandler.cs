using CSharpFunctionalExtensions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Models.Responses;
using System.Diagnostics.Contracts;
using System.Text.Json;
using User.Application.Helper.Implementation;
using User.Application.Interfaces;
using User.Domain.Entities;
using User.Domain.Interfaces.Messaging;
using User.Domain.Models.Requests;

namespace User.Application.Command
{
    public class PostUserCommandHandler : IRequestHandler<PostUserCommand, Result<ApiResponse>>
    {
        private readonly ILogger<PostUserCommandHandler> _logger;
        private readonly IUserService _userService;
        private readonly IValidator<CreateUserRequest> _validator;
        private readonly IMessageProducer _messageProducer;

        public PostUserCommandHandler(
            ILogger<PostUserCommandHandler> logger,
            IUserService userService,
            IValidator<CreateUserRequest> validator,
            IMessageProducer messageProducer)
        {
            _logger = logger;
            _userService = userService;
            _validator = validator;
            _messageProducer = messageProducer;
        }

        public async Task<Result<ApiResponse>> Handle(PostUserCommand request, CancellationToken cancellationToken)
        {
            Contract.Assert(request != null);

            _logger.LogTrace("Executing handler for request : {request}", nameof(PostUserCommand));

            try
            {
                ValidationResult validationResult = await _validator.ValidateAsync(request.Request);

                if (!validationResult.IsValid)
                {
                    return ResponseHelper.Failed("Error validation", validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));
                }

                Users user = new Users();
                user.Name = request.Request.Name;
                user.Email = request.Request.Email;

                var userData = await _userService.CreateUserAsync(user, cancellationToken);

                if (userData == null) return ResponseHelper.Failed("Create user data failed!");

                // Publish to Kafka
                var json = JsonSerializer.Serialize(userData);
                await _messageProducer.PublishAsync("users-created", json, cancellationToken);

                return ResponseHelper.Success(userData);
            }
            catch (Exception ex)
            {
                return ResponseHelper.Failed(ex.Message);
            }
        }
    }
}