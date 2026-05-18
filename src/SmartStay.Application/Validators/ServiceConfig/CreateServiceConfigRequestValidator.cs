using FluentValidation;
using SmartStay.Application.DTOs.ServiceConfig;

namespace SmartStay.Application.Validators.ServiceConfig;

public class CreateServiceConfigRequestValidator : AbstractValidator<CreateServiceConfigRequest>
{
    public CreateServiceConfigRequestValidator()
    {
        RuleFor(x => x.PropertyId)
            .NotEmpty().WithMessage("PropertyId is required.");

        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("Type is required.")
            .MaximumLength(50).WithMessage("Type must not exceed 50 characters.");

        RuleFor(x => x.UnitPrice)
            .GreaterThanOrEqualTo(0).WithMessage("UnitPrice must be greater than or equal to 0.");
    }
}
