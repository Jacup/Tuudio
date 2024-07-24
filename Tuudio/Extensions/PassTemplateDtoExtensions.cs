using Tuudio.Domain.Entities.Activities;
using Tuudio.Domain.Entities.PassTemplates;
using Tuudio.DTOs;

namespace Tuudio.Extensions;

public static class PassTemplateDtoExtensions
{
    public static PassTemplate FromDto(this PassTemplateDto dto, IEnumerable<Activity> activities) => dto.FromDto(Guid.NewGuid(), activities);

    public static PassTemplate FromDto(this PassTemplateDto dto, Guid id, IEnumerable<Activity> activities) => new()
    {
        Id = id,

        Name = dto.Name,
        Description = dto.Description,
        Entries = dto.Entries,

        Price = new Price()
        {
            Amount = dto.Price_Amount,
            Period = dto.Price_Period
        },

        Duration = new()
        {
            Amount = dto.Duration_Amount,
            Period = dto.Duration_Period
        },

        Activities = activities.ToList(),
    };

    public static PassTemplateDto ToDto(this PassTemplate passTemplate) => new()
    {
        Name = passTemplate.Name,
        Description = passTemplate.Description,
        Entries = passTemplate.Entries,

        Price_Amount = passTemplate.Price.Amount,
        Price_Period = passTemplate.Price.Period,

        Duration_Amount = passTemplate.Duration.Amount,
        Duration_Period = passTemplate.Duration.Period,

        Activities = passTemplate.Activities.Select(activity => activity.Id),
    };

    public static PassTemplateDetailedDto ToDetailedDto(this PassTemplate passTemplate) => new()
    {
        Id = passTemplate.Id,

        CreatedDate = passTemplate.CreatedDate,
        UpdatedDate = passTemplate.UpdatedDate,

        Name = passTemplate.Name,
        Description = passTemplate.Description,
        Entries = passTemplate.Entries,

        Price_Amount = passTemplate.Price.Amount,
        Price_Period = passTemplate.Price.Period,

        Duration_Amount = passTemplate.Duration.Amount,
        Duration_Period = passTemplate.Duration.Period,

        Activities = passTemplate.Activities.Select(activity => activity.Id),
    };
}
