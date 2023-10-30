using FluentValidation;

namespace HealthERP.Application.Command.PolicyHolders.Validations
{
    public class CreatePolicyHolderValidations : AbstractValidator<CreatePoliyholderCommand>
    {
        public CreatePolicyHolderValidations() 
        {
            RuleFor(x => x.PolicyNumber)
                .NotNull()
                .NotEmpty()
                .Must(x => x.Any(x => Char.IsDigit(x))).WithMessage("invalid policy number:policy number must be alphanumeric")
                .Must(x => x.Any(x => Char.IsLetter(x))).WithMessage("invalid policy number:policy number must be alphanumeric");
        }
        
    }
}
