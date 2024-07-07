using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http.Json;
using System.Text.Json.Serialization;
using Tuudio.Endpoints;
using Tuudio.Validators;

namespace Tuudio.Extensions;

public static class Configuration
{
    public static void RegisterServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthorization();

        builder.Services.Configure<JsonOptions>(options =>
        {
            //options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        });

        builder.Services
            .AddFluentValidationAutoValidation()
            .AddValidatorsFromAssemblyContaining<ClientDtoValidator>();

        builder.Services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen(o => o.SupportNonNullableReferenceTypes())
            .AddLogging();
    }

    public static void RegisterMiddleware(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.MapGroup("/api/clients").MapClientsApi();
    }
}
