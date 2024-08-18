using Mapster;
using Tuudio.Domain.Entities.Activities;
using Tuudio.Domain.Entities.Entries;
using Tuudio.Domain.Entities.Passes;
using Tuudio.Domain.Entities.People;
using Tuudio.DTOs;
using Tuudio.DTOs.People;
using Tuudio.DTOs.People.Detailed;

namespace Tuudio.Services.Mapping;

public static class MapsterConfiguration
{
    public static void Configure()
    {
        // Clients
        TypeAdapterConfig<Client, ClientDetailedDto>.NewConfig()
            .Map(dest => dest.Passes, src => src.Passes.Select(pass => pass.Id).ToList())
            .Map(dest => dest.Entries, src => src.Entries.Select(entry => entry.Id).ToList());

        TypeAdapterConfig<ClientDto, Client>.NewConfig()
            .Ignore(dest => dest.Passes)
            .Ignore(dest => dest.Entries);



        // Activities
        TypeAdapterConfig<Activity, ActivityDetailedDto>.NewConfig()
            .Map(dest => dest.PassTemplates, src => src.PassTemplates.Select(pt => pt.Id).ToList())
            .Map(dest => dest.Entries, src => src.Entries.Select(entry => entry.Id).ToList());

        TypeAdapterConfig<ActivityDto, Activity>.NewConfig()
            .Ignore(dest => dest.PassTemplates)
            .Ignore(dest => dest.Entries);



        // PassTemplates
        TypeAdapterConfig<PassTemplateDto, PassTemplate>.NewConfig()
            .Ignore(dest => dest.Passes)
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
            .Map(dest => dest.Activities, src => src.Activities.Select(a => a.Id).ToList())
            .Map(dest => dest.Passes, src => src.Passes.Select(a => a.Id).ToList());



        // Passes
        TypeAdapterConfig<Pass, PassDetailedDto>.NewConfig()
            .Map(dest => dest.Entries, src => src.Entries.Select(entry => entry.Id).ToList());



        // Entries
        //TypeAdapterConfig<Entry, EntryDetailedDto>.NewConfig()
            //.Map(dest => dest.)





    }
}

