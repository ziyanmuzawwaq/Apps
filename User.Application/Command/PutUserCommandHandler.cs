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
using User.Domain.Interfaces.Messaging;
using User.Domain.Models.Requests;

namespace User.Application.Command
{
    public class PutUserCommandHandler : IRequestHandler<PutUserCommand, Result<ApiResponse>>
    {
        private readonly ILogger<PutUserCommandHandler> _logger;
        private readonly IUserService _userService;
        private readonly IValidator<UpdateUserRequest> _validator;
        private readonly IMessageProducer _messageProducer;

        public PutUserCommandHandler(
            ILogger<PutUserCommandHandler> logger,
            IUserService userService,
            IValidator<UpdateUserRequest> validator,
            IMessageProducer messageProducer)
        {
            _logger = logger;
            _userService = userService;
            _validator = validator;
            _messageProducer = messageProducer;
        }

        public async Task<Result<ApiResponse>> Handle(PutUserCommand request, CancellationToken cancellationToken)
        {
            Contract.Assert(request != null);

            _logger.LogTrace("Executing handler for request : {request}", nameof(PutUserCommand));

            try
            {
                ValidationResult validationResult = await _validator.ValidateAsync(request.Request);

                if (!validationResult.IsValid)
                {
                    return ResponseHelper.Failed("Error validation", validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));
                }

                var user = await _userService.GetUserByIdAsync(request.Request.UserId, cancellationToken);

                if (user == null) ResponseHelper.Failed("User not found!");

                user!.Name = string.IsNullOrWhiteSpace(request.Request.Name) ? user.Name : request.Request.Name;
                user.Email = string.IsNullOrWhiteSpace(request.Request.Email) ? user.Email : request.Request.Email;
                user.UpdatedAt = DateTime.UtcNow;

                var updatedUser = await _userService.UpdateUserAsync(user, cancellationToken);

                if (updatedUser == null) return ResponseHelper.Failed("Update user data failed!");

                // Publish to Kafka
                var json = JsonSerializer.Serialize(updatedUser);
                await _messageProducer.PublishAsync("users-updated", json, cancellationToken);

                return ResponseHelper.Success(updatedUser);
            }
            catch (Exception ex)
            {
                return ResponseHelper.Failed(ex.Message);
            }
        }
    }
}