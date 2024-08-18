using FluentValidation;
using FluentValidation.Results;
using Mapster;
using Tuudio.Domain.Entities.Passes;
using Tuudio.Domain.Exceptions;
using Tuudio.DTOs;
using Tuudio.Infrastructure.Services.Interfaces;

namespace Tuudio.Endpoints;

public static class PassApi
{
    private const string MissingDtoDataErrorMessage = "Pass data is required.";
    private const string MissingIdErrorMessage = "Pass id is required.";
    private const string Path = "passes";

    public static RouteGroupBuilder MapPassesApi(this RouteGroupBuilder group)
    {
        group
            .MapGet("/", async (IUnitOfWork uow) => await uow.ExecuteAsync(() => GetAsync(uow)))
            .Produces<IEnumerable<PassDetailedDto>>(StatusCodes.Status200OK)
            .WithOpenApi();

        group
            .MapGet("/{id}", async (Guid id, IUnitOfWork uow) => await uow.ExecuteAsync(() => GetByIdAsync(uow, id)))
            .Produces<PassDetailedDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi();

        group
            .MapPost("/", async (PassDto passDto, IValidator<PassDto> validator, IUnitOfWork uow) => await uow.ExecuteAsync(() => AddAsync(uow, passDto, validator)))
            .Produces<PassDetailedDto>(StatusCodes.Status201Created)
            .Produces<IEnumerable<ValidationFailure>>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi();

        group
            .MapPut("/{id}", async (Guid id, PassDto passDto, IValidator<PassDto> validator, IUnitOfWork uow) => await uow.ExecuteAsync(() => UpdateAsync(uow, id, passDto, validator)))
            .Produces<PassDetailedDto>(StatusCodes.Status200OK)
            .Produces<IEnumerable<ValidationFailure>>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi();

        group
            .MapDelete("/{id}", async (Guid id, IUnitOfWork uow) => await uow.ExecuteAsync(() => DeleteAsync(uow, id)))
            .Produces(StatusCodes.Status204NoContent)
            .Produces<string>(StatusCodes.Status404NotFound)
            .WithOpenApi();

        return group.WithTags(Path);
    }

    internal static async Task<IResult> GetAsync(IUnitOfWork uow)
    {
        var passes = await uow.PassRepository.GetAllAsync();

        return Results.Ok(passes.Select(pt => pt.Adapt<PassDetailedDto>()));
    }

    internal static async Task<IResult> GetByIdAsync(IUnitOfWork uow, Guid id)
    {
        var pass = await uow.PassRepository.GetByIdAsync(id);

        if (pass == null)
            return Results.NotFound($"Pass with ID \"{id}\" not found");

        return Results.Ok(pass.Adapt<PassDetailedDto>());
    }

    internal static async Task<IResult> AddAsync(IUnitOfWork uow, PassDto dto, IValidator<PassDto> validator)
    {
        if (dto == null)
            return Results.BadRequest(MissingDtoDataErrorMessage);

        var validationResult = await validator.ValidateAsync(dto);

        if (!validationResult.IsValid)
            return Results.BadRequest(validationResult.Errors);

        var pass = dto.Adapt<Pass>();
        pass.Id = Guid.NewGuid();

        await uow.PassRepository.InsertAsync(pass);

        return Results.Created($"/{Path}/{pass.Id}", pass.Adapt<PassDetailedDto>());
    }
    
    internal static async Task<IResult> UpdateAsync(IUnitOfWork uow, Guid id, PassDto dto, IValidator<PassDto> validator)
    {
        if (id == Guid.Empty)
            return Results.BadRequest(MissingIdErrorMessage);

        if (dto == null)
            return Results.BadRequest(MissingDtoDataErrorMessage);

        var validationResult = await validator.ValidateAsync(dto);

        if (!validationResult.IsValid)
            return Results.BadRequest(validationResult.Errors);

        var pass = dto.Adapt<Pass>();
        pass.Id = id;

        try
        {
            await uow.PassRepository.UpdateAsync(pass);
        }
        catch (EntityNotFoundException<Pass> e)
        {
            return Results.NotFound(e.Message);
        }

        return Results.Ok(pass.Adapt<PassDetailedDto>());
    }

    internal static async Task<IResult> DeleteAsync(IUnitOfWork uow, Guid id)
    {
        try
        {
            await uow.PassRepository.DeleteAsync(id);
        }
        catch (EntityNotFoundException<Pass> e)
        {
            return Results.NotFound(e.Message);
        }

        return Results.NoContent();
    }
}
