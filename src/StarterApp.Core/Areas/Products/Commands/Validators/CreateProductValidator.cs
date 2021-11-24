using FluentValidation;

namespace StarterApp.Core.Areas.Products.Commands.Validators
{
    public class CreateProductValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductValidator()
        {
            RuleFor(v => v.CategoryId)
                .NotEmpty().WithMessage("CategoryId is required.");
            RuleFor(v => v.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Price should be positive value.");
        }
    }
}
