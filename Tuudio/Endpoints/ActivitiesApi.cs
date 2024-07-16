using FluentValidation;
using FluentValidation.Results;
using Tuudio.Domain.Entities.Activities;
using Tuudio.Domain.Exceptions;
using Tuudio.DTOs;
using Tuudio.Extensions;
using Tuudio.Infrastructure.Services.Interfaces;

namespace Tuudio.Endpoints;

public static class ActivitiesApi
{
    public static RouteGroupBuilder MapActivitiesApi(this RouteGroupBuilder group)
    {
        group
            .MapGet("/", async (IUnitOfWork uow) => await uow.ExecuteAsync(() => GetAsync(uow)))
            .Produces<IEnumerable<ActivityDetailedDto>>(StatusCodes.Status200OK)
            .WithOpenApi();

        group
            .MapGet("/{id}", async (Guid id, IUnitOfWork uow) => await uow.ExecuteAsync(() => GetByIdAsync(uow, id)))
            .Produces<ActivityDetailedDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi();

        group
            .MapPost("/", async (ActivityDto activityDto, IValidator<ActivityDto> validator, IUnitOfWork uow) => await uow.ExecuteAsync(() => AddAsync(uow, activityDto, validator)))
            .Produces<ActivityDto>(StatusCodes.Status201Created)
            .Produces<IEnumerable<ValidationFailure>>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi();

        group
            .MapPut("/{id}", async (Guid id, ActivityDto activityDto, IValidator<ActivityDto> validator, IUnitOfWork uow) => await uow.ExecuteAsync(() => UpdateAsync(uow, id, activityDto, validator)))
            .Produces<ActivityDto>(StatusCodes.Status200OK)
            .Produces<IEnumerable<ValidationFailure>>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi();

        group
            .MapDelete("/{id}", async (Guid id, IUnitOfWork uow) => await uow.ExecuteAsync(() => DeleteAsync(uow, id)))
            .Produces(StatusCodes.Status204NoContent)
            .Produces<string>(StatusCodes.Status404NotFound)
            .WithOpenApi();

        return group.WithTags("activities");
    }

    internal static async Task<IResult> GetAsync(IUnitOfWork uow)
    {
        var activities = await uow.ActivityRepository.GetAllAsync();

        return Results.Ok(activities.Select(a => a.ToDetailedDto()));
    }

    internal static async Task<IResult> GetByIdAsync(IUnitOfWork uow, Guid id)
    {
        var activity = await uow.ActivityRepository.GetByIdAsync(id);

        if (activity == null)
            return Results.NotFound($"Activity with ID \"{id}\" not found");

        return Results.Ok(activity.ToDetailedDto());
    }

    internal static async Task<IResult> AddAsync(IUnitOfWork uow, ActivityDto dto, IValidator<ActivityDto> validator)
    {
        if (dto == null)
            return Results.BadRequest("Activity data is required.");

        var validationResult = await validator.ValidateAsync(dto);

        if (!validationResult.IsValid)
            return Results.BadRequest(validationResult.Errors);

        var activity = dto.FromDto();

        await uow.ActivityRepository.InsertAsync(activity);

        return Results.Created($"/activities/{activity.Id}", activity.ToDetailedDto());
    }

    internal static async Task<IResult> UpdateAsync(IUnitOfWork uow, Guid id, ActivityDto dto, IValidator<ActivityDto> validator)
    {
        if (id == Guid.Empty)
            return Results.BadRequest("Activity id is required.");

        if (dto == null)
            return Results.BadRequest("Activity data is required.");

        var validationResult = await validator.ValidateAsync(dto);

        if (!validationResult.IsValid)
            return Results.BadRequest(validationResult.Errors);

        var activity = dto.FromDto(id);

        try
        {
            await uow.ActivityRepository.UpdateAsync(activity);
        }
        catch (EntityNotFoundException<Activity> e)
        {
            return Results.NotFound(e.Message);
        }

        return Results.Ok(activity.ToDetailedDto());
    }

    internal static async Task<IResult> DeleteAsync(IUnitOfWork uow, Guid id)
    {
        try
        {
            await uow.ActivityRepository.DeleteAsync(id);
        }
        catch (EntityNotFoundException<Activity> e)
        {
            return Results.NotFound(e.Message);
        }

        return Results.NoContent();
    }
}
