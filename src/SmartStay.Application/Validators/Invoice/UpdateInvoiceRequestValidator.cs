using FluentValidation;
using SmartStay.Application.DTOs.Invoice;

namespace SmartStay.Application.Validators.Invoice;

public class UpdateInvoiceRequestValidator : AbstractValidator<UpdateInvoiceRequest>
{
    public UpdateInvoiceRequestValidator()
    {
        RuleFor(x => x.PaidAmount)
            .GreaterThanOrEqualTo(0).WithMessage("PaidAmount must be greater than or equal to 0.");
    }
}
