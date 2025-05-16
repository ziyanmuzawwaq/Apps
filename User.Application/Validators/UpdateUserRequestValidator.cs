﻿using FluentValidation;
using User.Domain.Models.Requests;

namespace User.Application.Validators
{
    public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
    {
        public CreateUserRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Email).NotEmpty().MaximumLength(100);
        }
    }
}