using Application.Requests.Products;
using FastEndpoints;
using FluentValidation;

namespace Application.Validators;

public class CreateProductRequestValidator : Validator<CreateProductRequest>
{
    public CreateProductRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Product name is required.")
            .MaximumLength(100).WithMessage("Product name must not exceed 100 characters.");
    }
}