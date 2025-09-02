using FluentValidation;
using static Catalog.Api.Dtos.Product_DTO;

namespace Catalog.Api.Validators;

public class CreateProductValidator: AbstractValidator<CreateProductDto>
{
    public CreateProductValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
        RuleFor(x => x.Stock).GreaterThanOrEqualTo(0).WithMessage("Stock cannot be negative");
    }

}