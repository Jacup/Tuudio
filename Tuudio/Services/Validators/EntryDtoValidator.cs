using FluentValidation;
using Tuudio.DTOs;

namespace Tuudio.Services.Validators;

public class EntryDtoValidator : AbstractValidator<EntryDto>
{
    public EntryDtoValidator()
    {
        RuleFor(x => x.EntryDate)
            .NotEmpty();

        RuleFor(x => x.ClientId)
            .NotEmpty();

        RuleFor(x => x.Note)
            .MaximumLength(128);
    }
}
