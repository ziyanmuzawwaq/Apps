using FluentValidation;
using User.Domain.Models.Requests;

namespace User.Application.Validators
{
    public class UpdateUserRequestValidatorcs : AbstractValidator<UpdateUserRequest>
    {
        public UpdateUserRequestValidatorcs()
        {
            RuleFor(x => x.UserId).NotNull().NotEmpty();
            RuleFor(x => x.Name).NotNull().NotEmpty().MaximumLength(100);
            RuleFor(x => x.Email).NotNull().NotEmpty().MaximumLength(100);
        }
    }
}