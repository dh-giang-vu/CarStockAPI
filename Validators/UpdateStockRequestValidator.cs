namespace CarStockApi.Validators;

using CarStockApi.Dto.Request.Car;
using FastEndpoints;
using FluentValidation;

public class UpdateStockRequestValidator : Validator<UpdateStockRequest>
{
    public UpdateStockRequestValidator()
    {
        RuleFor(x => x.CarId)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Car ID starts from 1.");

        RuleFor(x => x.NewStockLevel)
            .GreaterThanOrEqualTo(0)
            .WithMessage("New stock level cannot be negative.");
    }
}
