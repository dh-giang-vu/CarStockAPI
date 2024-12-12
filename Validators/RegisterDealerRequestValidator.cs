namespace CarStockApi.Validators;

using CarStockApi.Dto.Request.Dealer;
using FastEndpoints;
using FluentValidation;

public class RegisterDealerRequestValidator : Validator<RegisterDealerRequest>
{
    public RegisterDealerRequestValidator()
    {
        RuleFor(x => x.Credentials.Username)
            .NotEmpty()
            .WithMessage("Username is required.")
            .MinimumLength(6)
            .WithMessage("Username must be minimum 6 characters long.");

        RuleFor(x => x.Credentials.Password)
            .NotEmpty()
            .WithMessage("Password is required.")
            .MinimumLength(6)
            .WithMessage("Password must be minimum 6 characters long.");
    }
}
