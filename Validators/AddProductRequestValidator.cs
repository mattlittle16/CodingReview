using CodingTest.Models;
using FluentValidation;

namespace CodingTest.Validators;

public class AddProductRequestValidator : AbstractValidator<AddProductRequestModel>
{
    public AddProductRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Product name is required")
            .MaximumLength(20)
            .WithMessage("Product name cannot exceed 20 characters");

        RuleFor(x => x.Price)
            .GreaterThan(-1)
            .WithMessage("Price cannot be negative");            
    }
}