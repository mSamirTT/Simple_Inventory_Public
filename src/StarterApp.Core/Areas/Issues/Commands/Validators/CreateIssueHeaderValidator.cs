using FluentValidation;

namespace StarterApp.Core.Areas.Issues.Commands.Validators
{
    public class CreateIssueHeaderValidator : AbstractValidator<CreateIssueHeaderCommand>
    {
        public CreateIssueHeaderValidator()
        {
        }
    }
}
