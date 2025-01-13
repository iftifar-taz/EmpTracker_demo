using EmpTracker.Identity.Application.Features.Sessions.Commands;
using FluentValidation;

namespace EmpTracker.Identity.Application.Behaviors.Validators
{
    public class RefreshSessionCommandValidator : AbstractValidator<RefreshSessionCommand>
    {
        public RefreshSessionCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required.")
                .EmailAddress()
                .WithMessage("Invalid email address format.");

            RuleFor(x => x.Token)
                .NotEmpty()
                .WithMessage("Token is required.");

            RuleFor(x => x.RefreshToken)
                .NotEmpty()
                .WithMessage("Refresh Token is required.");
        }
    }
}
