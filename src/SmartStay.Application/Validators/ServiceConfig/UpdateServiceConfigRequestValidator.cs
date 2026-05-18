using FluentValidation;
using SmartStay.Application.DTOs.ServiceConfig;

namespace SmartStay.Application.Validators.ServiceConfig;

public class UpdateServiceConfigRequestValidator : AbstractValidator<UpdateServiceConfigRequest>
{
    public UpdateServiceConfigRequestValidator()
    {
        RuleFor(x => x.UnitPrice)
            .GreaterThanOrEqualTo(0).WithMessage("UnitPrice must be greater than or equal to 0.");
    }
}
