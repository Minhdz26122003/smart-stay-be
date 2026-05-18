using FluentValidation;
using SmartStay.Application.DTOs.MeterReading;

namespace SmartStay.Application.Validators.MeterReading;

public class CreateMeterReadingRequestValidator : AbstractValidator<CreateMeterReadingRequest>
{
    public CreateMeterReadingRequestValidator()
    {
        RuleFor(x => x.RoomId)
            .NotEmpty().WithMessage("RoomId is required.");

        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("Type is required.")
            .MaximumLength(50).WithMessage("Type must not exceed 50 characters.");

        RuleFor(x => x.OldUnit)
            .GreaterThanOrEqualTo(0).WithMessage("OldUnit must be greater than or equal to 0.");

        RuleFor(x => x.NewUnit)
            .GreaterThanOrEqualTo(x => x.OldUnit).WithMessage("NewUnit must be greater than or equal to OldUnit.");

        RuleFor(x => x.Month)
            .InclusiveBetween(1, 12).WithMessage("Month must be between 1 and 12.");

        RuleFor(x => x.Year)
            .GreaterThanOrEqualTo(2000).WithMessage("Year must be 2000 or later.");
    }
}
