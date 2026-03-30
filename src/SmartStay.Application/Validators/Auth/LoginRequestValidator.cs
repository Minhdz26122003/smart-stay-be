using FluentValidation;
using SmartStay.Application.DTOs.Auth;

namespace SmartStay.Application.Validators.Auth;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.PhoneOrEmail)
            .NotEmpty().WithMessage("Phone or Email is required.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.");
    }
}
