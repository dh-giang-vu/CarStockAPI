namespace CarStockApi.Validators;

using CarStockApi.Dto.Request.Car;
using FastEndpoints;
using FluentValidation;

public class AddCarRequestValidator : Validator<AddCarRequest>
{
    public AddCarRequestValidator()
    {
        RuleFor(x => x.Year)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Year is not in appropriate range.")
            .LessThanOrEqualTo(DateTime.Now.Year)
            .WithMessage("Year is not in appropriate range.");

        RuleFor(x => x.StockLevel)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Stock level cannot be negative.");
    }
}
