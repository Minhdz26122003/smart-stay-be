using FluentValidation;
using SmartStay.Application.DTOs.Vehicle;

namespace SmartStay.Application.Validators.Vehicle;

public class CreateVehicleRequestValidator : AbstractValidator<CreateVehicleRequest>
{
    public CreateVehicleRequestValidator()
    {
        RuleFor(x => x.PlateNumber)
            .NotEmpty().WithMessage("PlateNumber is required.")
            .MaximumLength(20).WithMessage("PlateNumber must not exceed 20 characters.");
    }
}
