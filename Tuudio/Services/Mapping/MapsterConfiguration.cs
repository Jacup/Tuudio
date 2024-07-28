using Mapster;
using Tuudio.Domain.Entities.Activities;
using Tuudio.Domain.Entities.PassTemplates;
using Tuudio.DTOs;

namespace Tuudio.Services.Mapping;

public static class MapsterConfiguration
{
    public static void Configure()
    {
        // Activities
        TypeAdapterConfig<ActivityDto, Activity>.NewConfig()
            .Ignore(dest => dest.PassTemplates);

        TypeAdapterConfig<Activity, ActivityDetailedDto>.NewConfig()
            .Map(dest => dest.PassTemplates, src => src.PassTemplates.Select(pt => pt.Id).ToList());

        // PassTemplates
        TypeAdapterConfig<PassTemplateDto, PassTemplate>.NewConfig()
            .Ignore(dest => dest.Activities)
            .Map(dest => dest.Price.Amount, src => src.Price_Amount)
            .Map(dest => dest.Price.Period, src => src.Price_Period)
            .Map(dest => dest.Duration.Amount, src => src.Duration_Amount)
            .Map(dest => dest.Duration.Period, src => src.Duration_Period);


        TypeAdapterConfig<PassTemplate, PassTemplateDetailedDto>.NewConfig()
            .Map(dest => dest.Price_Amount, src => src.Price.Amount)
            .Map(dest => dest.Price_Period, src => src.Price.Period)
            .Map(dest => dest.Duration_Amount, src => src.Duration.Amount)
            .Map(dest => dest.Duration_Period, src => src.Duration.Period)
            .Map(dest => dest.Activities, src => src.Activities.Select(a => a.Id).ToList());
    }
}

