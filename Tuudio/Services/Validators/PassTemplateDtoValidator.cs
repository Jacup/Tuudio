using FluentValidation;
using Tuudio.DTOs;

namespace Tuudio.Services.Validators;

public class PassTemplateDtoValidator : AbstractValidator<PassTemplateDto>
{
    public PassTemplateDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Pass Template name is required");

        RuleFor(x => x.Description)
            .NotEmpty()
            .Length(1, 128)
            .WithMessage("Activity name should be between 1 and 128 characters long.");
    }
}
