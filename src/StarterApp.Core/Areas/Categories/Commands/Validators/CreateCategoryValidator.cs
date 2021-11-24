using FluentValidation;

namespace StarterApp.Core.Areas.Categories.Commands.Validators
{
    public class CreateCategoryValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryValidator()
        {
        }
    }
}
