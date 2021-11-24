using FluentValidation;

namespace StarterApp.Core.Areas.Supplies.Commands.Validators
{
    public class CreateSupplyHeaderValidator : AbstractValidator<CreateSupplyHeaderCommand>
    {
        public CreateSupplyHeaderValidator()
        {
        }
    }
}
