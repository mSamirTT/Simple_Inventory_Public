using FluentValidation;

namespace StarterApp.Core.Areas.Supplies.Commands.Validators
{
    public class UpdateSupplyHeaderValidator : AbstractValidator<UpdateSupplyHeaderCommand>
    {
        public UpdateSupplyHeaderValidator()
        {
            RuleFor(v => v.Id)
                .NotEmpty().WithMessage("Id is required.");
        }
    }
}
