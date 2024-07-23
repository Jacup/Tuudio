﻿using FluentValidation;
using FluentValidation.Results;
using Tuudio.Domain.Entities.PassTemplates;
using Tuudio.Domain.Exceptions;
using Tuudio.DTOs;
using Tuudio.Extensions;
using Tuudio.Infrastructure.Services.Interfaces;

namespace Tuudio.Endpoints;

public static class PassTemplatesApi
{
    public static RouteGroupBuilder MapPassTemplatesApi(this RouteGroupBuilder group)
    {
        group
            .MapGet("/", async (IUnitOfWork uow) => await uow.ExecuteAsync(() => GetAsync(uow)))
            .Produces<IEnumerable<PassTemplateDetailedDto>>(StatusCodes.Status200OK)
            .WithOpenApi();

        group
            .MapGet("/{id}", async (Guid id, IUnitOfWork uow) => await uow.ExecuteAsync(() => GetByIdAsync(uow, id)))
            .Produces<PassTemplateDetailedDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi();

        group
            .MapPost("/", async (PassTemplateDto passTemplateDto, IValidator<PassTemplateDto> validator, IUnitOfWork uow) => await uow.ExecuteAsync(() => AddAsync(uow, passTemplateDto, validator)))
            .Produces<PassTemplateDetailedDto>(StatusCodes.Status201Created)
            .Produces<IEnumerable<ValidationFailure>>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi();

        group
            .MapPut("/{id}", async (Guid id, PassTemplateDto passTemplateDto, IValidator<PassTemplateDto> validator, IUnitOfWork uow) => await uow.ExecuteAsync(() => UpdateAsync(uow, id, passTemplateDto, validator)))
            .Produces<PassTemplateDetailedDto>(StatusCodes.Status200OK)
            .Produces<IEnumerable<ValidationFailure>>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi();

        group
            .MapDelete("/{id}", async (Guid id, IUnitOfWork uow) => await uow.ExecuteAsync(() => DeleteAsync(uow, id)))
            .Produces(StatusCodes.Status204NoContent)
            .Produces<string>(StatusCodes.Status404NotFound)
            .WithOpenApi();

        return group.WithTags("passTemplates");
    }

    internal static async Task<IResult> GetAsync(IUnitOfWork uow)
    {
        var passTemplates = await uow.PassTemplateRepository.GetAllAsync();

        return Results.Ok(passTemplates.Select(pt => pt.ToDetailedDto()));
    }

    internal static async Task<IResult> GetByIdAsync(IUnitOfWork uow, Guid id)
    {
        var passTemplate = await uow.PassTemplateRepository.GetByIdAsync(id);

        if (passTemplate == null)
            return Results.NotFound($"PassTemplate with ID \"{id}\" not found");

        return Results.Ok(passTemplate.ToDetailedDto());
    }

    internal static async Task<IResult> AddAsync(IUnitOfWork uow, PassTemplateDto dto, IValidator<PassTemplateDto> validator)
    {
        if (dto == null)
            return Results.BadRequest("PassTemplate data is required.");

        var validationResult = await validator.ValidateAsync(dto);

        if (!validationResult.IsValid)
            return Results.BadRequest(validationResult.Errors);

        var activities = await uow.ActivityRepository.GetByIdsAsync(dto.Activities);

        var passTemplate = dto.FromDto(activities);

        await uow.PassTemplateRepository.InsertAsync(passTemplate);

        return Results.Created($"/passTemplates/{passTemplate.Id}", passTemplate.ToDetailedDto());
    }

    internal static async Task<IResult> UpdateAsync(IUnitOfWork uow, Guid id, PassTemplateDto dto, IValidator<PassTemplateDto> validator)
    {
        if (id == Guid.Empty)
            return Results.BadRequest("PassTemplate id is required.");

        if (dto == null)
            return Results.BadRequest("PassTemplate data is required.");

        var validationResult = await validator.ValidateAsync(dto);

        if (!validationResult.IsValid)
            return Results.BadRequest(validationResult.Errors);

        var activities = await uow.ActivityRepository.GetByIdsAsync(dto.Activities);

        var passTemplate = dto.FromDto(id, activities);

        try
        {
            await uow.PassTemplateRepository.UpdateAsync(passTemplate);
        }
        catch (EntityNotFoundException<PassTemplate> e)
        {
            return Results.NotFound(e.Message);
        }

        return Results.Ok(passTemplate.ToDetailedDto());
    }

    internal static async Task<IResult> DeleteAsync(IUnitOfWork uow, Guid id)
    {
        try
        {
            await uow.PassTemplateRepository.DeleteAsync(id);
        }
        catch (EntityNotFoundException<PassTemplate> e)
        {
            return Results.NotFound(e.Message);
        }

        return Results.NoContent();
    }
}
