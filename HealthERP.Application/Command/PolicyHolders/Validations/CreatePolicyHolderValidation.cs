using FluentValidation;

namespace HealthERP.Application.Command.PolicyHolders.Validations
{
    public class CreatePolicyHolderValidation : AbstractValidator<CreatePolicyHolder.Request>
    {
        public CreatePolicyHolderValidation() 
        {
            RuleFor(x => x.PolicyNumber)
                  .NotEmpty()
                  .NotNull()
                  .Must(x => x.Any(x => Char.IsDigit(x)))
                  .Must(x => x.Any(x => Char.IsLetter(x)));
        }
    }
}
