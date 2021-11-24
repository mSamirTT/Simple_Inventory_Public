using FluentValidation;

namespace StarterApp.Core.Areas.Products.Commands.Validators
{
    public class UpdateProductValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductValidator()
        {
            RuleFor(v => v.Id)
                .NotEmpty().WithMessage("Id is required.");
            RuleFor(v => v.CategoryId)
                .NotEmpty().WithMessage("CategoryId is required.");
            RuleFor(v => v.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Price should be positive value.");
        }
    }
}
