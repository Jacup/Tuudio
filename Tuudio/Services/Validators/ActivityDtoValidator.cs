using FluentValidation;
using Tuudio.DTOs;

namespace Tuudio.Services.Validators;

public class ActivityDtoValidator : AbstractValidator<ActivityDto>
{
    public ActivityDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Activity name is required");

        RuleFor(x => x.Name)
            .Length(1, 64)
            .WithMessage("Activity name should be between 1 and 64 characters long.");
    }
}
