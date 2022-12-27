using Catalog.API.Infrastructure.DTOs;
using FluentValidation;

namespace Catalog.API.Infrastructure.Validators;

public class BaseProductValidator<T> : AbstractValidator<T> where T : BaseProductDto
{
    public BaseProductValidator()
    {
        RuleFor(p => p.Title).NotNull().NotEmpty().Length(3, 200);
        RuleFor(p => p.Description).Length(0, 1000);
        RuleFor(p => p.StockQuantity).NotNull();
    }
}

public class CreateProductValidator : BaseProductValidator<CreateProductDto>
{
    public CreateProductValidator()
    {
        RuleFor(p => p.CategoryId).NotNull().NotEmpty();
    }
}

public class UpdateProductValidator : BaseProductValidator<UpdateProductDto>
{
    public UpdateProductValidator()
    {
        RuleFor(p => p.Id).NotNull().NotEmpty();
        RuleFor(p => p.CategoryId).NotNull().NotEmpty();
    }
}