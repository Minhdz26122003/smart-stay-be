using FluentValidation;
using SmartStay.Application.DTOs.Invoice;

namespace SmartStay.Application.Validators.Invoice;

public class CreateInvoiceRequestValidator : AbstractValidator<CreateInvoiceRequest>
{
    public CreateInvoiceRequestValidator()
    {
        RuleFor(x => x.RoomId)
            .NotEmpty().WithMessage("RoomId is required.");

        RuleFor(x => x.ContractId)
            .NotEmpty().WithMessage("ContractId is required.");

        RuleFor(x => x.Month)
            .InclusiveBetween(1, 12).WithMessage("Month must be between 1 and 12.");

        RuleFor(x => x.Year)
            .GreaterThanOrEqualTo(2000).WithMessage("Year must be 2000 or later.");

        RuleFor(x => x.BreakdownJson)
            .NotEmpty().WithMessage("BreakdownJson is required.");

        RuleFor(x => x.TotalAmount)
            .GreaterThanOrEqualTo(0).WithMessage("TotalAmount must be greater than or equal to 0.");
    }
}
