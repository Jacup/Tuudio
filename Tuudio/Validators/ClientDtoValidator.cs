using FluentValidation;
using Tuudio.Models.DTOs;

namespace Tuudio.Validators;

public class ClientDtoValidator : AbstractValidator<ClientDto>
{
    public ClientDtoValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.");
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.");
        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("A valid email is required.");
        RuleFor(x => x.PhoneNumber)
            .Matches(@"^(\+?\d{2})(\d{9})$").WithMessage("Phone number is not in valid format (+00111222333)");
    }
}