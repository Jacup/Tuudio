using Tuudio.Domain.Entities.Activities;
using Tuudio.DTOs;

namespace Tuudio.Extensions;

public static class ActivityDtoExtensions
{
    public static Activity FromDto(this ActivityDto dto) => dto.FromDto(Guid.NewGuid());

    public static Activity FromDto(this ActivityDto dto, Guid id) => new()
    {
        Id = id,

        Name = dto.Name,
        Description = dto.Description,
    };

    public static ActivityDto ToDto(this Activity activity) => new()
    {
        Name = activity.Name,
        Description = activity.Description,
    };

    public static ActivityDetailedDto ToDetailedDto(this Activity activity) => new()
    {
        Id = activity.Id,

        CreatedDate = activity.CreatedDate,
        UpdatedDate = activity.UpdatedDate,

        Name = activity.Name,
        Description = activity.Description,
    };
}
