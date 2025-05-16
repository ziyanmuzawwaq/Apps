using Donate.Domain.Models.Requests;
using FluentValidation;

namespace Donate.Application.Validators
{
    public class CreateDonationRequestValidator : AbstractValidator<CreateDonationRequest>
    {
        public CreateDonationRequestValidator()
        {
            RuleFor(x => x.DonorName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.DonorEmail).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Amount).NotEmpty();
        }
    }
}