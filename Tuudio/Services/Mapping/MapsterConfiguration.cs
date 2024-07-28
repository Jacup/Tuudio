using Mapster;
using Tuudio.Domain.Entities.Activities;
using Tuudio.DTOs;

namespace Tuudio.Services.Mapping;

public static class MapsterConfiguration
{
    public static void Configure()
    {
        TypeAdapterConfig<ActivityDto, Activity>.NewConfig().Ignore(dest => dest.PassTemplates);

        TypeAdapterConfig<Activity, ActivityDetailedDto>.NewConfig().Map(dest => dest.PassTemplates, src => src.PassTemplates.Select(pt => pt.Id).ToList());
    }
}

