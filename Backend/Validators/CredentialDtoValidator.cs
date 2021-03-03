using Files.Management.Dtos;
using FluentValidation;
namespace Files.Management.Validators
{
    public class CredentialDtoValidator : AbstractValidator<CredentialDto>
    {
        public CredentialDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .NotNull()
                .EmailAddress();

            RuleFor(x => x.Password)
                .NotEmpty()
                .NotNull()
                .MinimumLength(6);
        }
    }
}
