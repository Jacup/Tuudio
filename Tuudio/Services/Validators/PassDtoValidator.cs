using FluentValidation;
using Tuudio.DTOs;

namespace Tuudio.Services.Validators;

public class PassDtoValidator : AbstractValidator<PassDto>
{
    public PassDtoValidator()
    {
        RuleFor(x => x.ClientId)
            .NotEmpty();
        
        RuleFor(x => x.PassTemplateId)
            .NotEmpty();
    }
}
