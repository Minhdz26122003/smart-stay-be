using FluentValidation;
using SmartStay.Application.DTOs.Roommate;

namespace SmartStay.Application.Validators.Roommate;

public class CreateRoommateRequestValidator : AbstractValidator<CreateRoommateRequest>
{
    public CreateRoommateRequestValidator()
    {
        RuleFor(x => x.ContractId)
            .NotEmpty().WithMessage("ContractId is required.");

        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("FullName is required.")
            .MaximumLength(100).WithMessage("FullName must not exceed 100 characters.");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone is required.")
            .MaximumLength(20).WithMessage("Phone must not exceed 20 characters.");
    }
}
