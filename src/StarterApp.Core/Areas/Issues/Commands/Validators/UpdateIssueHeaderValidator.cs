using FluentValidation;

namespace StarterApp.Core.Areas.Issues.Commands.Validators
{
    public class UpdateIssueHeaderValidator : AbstractValidator<UpdateIssueHeaderCommand>
    {
        public UpdateIssueHeaderValidator()
        {
            RuleFor(v => v.Id)
                .NotEmpty().WithMessage("Id is required.");
        }
    }
}
