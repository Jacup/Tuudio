using FluentValidation;
using Tuudio.DTOs.People;

namespace Tuudio.Services.Validators;

public class ClientDtoValidator : AbstractValidator<ClientDto>
{
    public ClientDtoValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().WithMessage($"First Name is required.");
        RuleFor(x => x.FirstName).MinimumLength(1).WithMessage("The length of 'First Name' must be greater than 0.");
        RuleFor(x => x.FirstName).MaximumLength(64).WithMessage("The length of 'First Name' must be lower than 64.");

        RuleFor(x => x.LastName).NotEmpty().WithMessage("Last Name is required.");
        RuleFor(x => x.LastName).MinimumLength(1).WithMessage("The length of 'Last Name' must be greater than 0.");
        RuleFor(x => x.LastName).MaximumLength(64).WithMessage("The length of 'Last Name' must be lower than 64.");

        RuleFor(x => x.Email).EmailAddress().WithMessage("A valid email is required.");
        
        RuleFor(x => x.PhoneNumber).Matches(@"^(\+?\d{2})(\d{9})$").WithMessage("Phone number is not in valid format (+00111222333)");
    }
}