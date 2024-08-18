using FluentValidation;
using FluentValidation.Results;
using Mapster;
using Tuudio.Domain.Entities.Entries;
using Tuudio.Domain.Exceptions;
using Tuudio.DTOs;
using Tuudio.Infrastructure.Services.Interfaces;

namespace Tuudio.Endpoints;

public static class EntriesApi
{
    private const string Path = "entries";

    public static RouteGroupBuilder MapEntriesApi(this RouteGroupBuilder group)
    {
        group
            .MapGet("/", (IUnitOfWork uow) => uow.ExecuteAsync(() => GetAsync(uow)))
            .Produces<IEnumerable<EntryDetailedDto>>(StatusCodes.Status200OK)
            .WithOpenApi();

        group
            .MapGet("/{id}", (Guid id, IUnitOfWork uow) => uow.ExecuteAsync(() => GetByIdAsync(uow, id)))
            .Produces<EntryDetailedDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi();

        group
            .MapPost("/", (EntryDto entryDto, IValidator<EntryDto> validator, IUnitOfWork uow) => uow.ExecuteAsync(() => AddAsync(uow, entryDto, validator)))
            .Produces<EntryDetailedDto>(StatusCodes.Status201Created)
            .Produces<IEnumerable<ValidationFailure>>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi();

        group
            .MapPut("/{id}", (Guid id, EntryDto entryDto, IValidator<EntryDto> validator, IUnitOfWork uow) => uow.ExecuteAsync(() => UpdateAsync(uow, id, entryDto, validator)))
            .Produces<EntryDetailedDto>(StatusCodes.Status200OK)
            .Produces<IEnumerable<ValidationFailure>>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi();

        group
            .MapDelete("/{id}", (Guid id, IUnitOfWork uow) => uow.ExecuteAsync(() => DeleteAsync(uow, id)))
            .Produces(StatusCodes.Status204NoContent)
            .Produces<string>(StatusCodes.Status404NotFound)
            .WithOpenApi();

        return group.WithTags(Path);
    }

    internal static async Task<IResult> GetAsync(IUnitOfWork unitOfWork)
    {
        var entries = await unitOfWork.EntryRepository.GetAllAsync();

        return Results.Ok(entries.Select(c => c.Adapt<EntryDetailedDto>()));
    }

    internal static async Task<IResult> GetByIdAsync(IUnitOfWork unitOfWork, Guid id)
    {
        var entry = await unitOfWork.EntryRepository.GetByIdAsync(id);

        if (entry == null)
            return Results.NotFound($"Entry with ID \"{id}\" not found");

        return Results.Ok(entry.Adapt<EntryDetailedDto>());
    }

    internal static async Task<IResult> AddAsync(IUnitOfWork unitOfWork, EntryDto entryDto, IValidator<EntryDto> validator)
    {
        if (entryDto == null)
            return Results.BadRequest("Entry data is required.");

        var validationResult = await validator.ValidateAsync(entryDto);

        if (!validationResult.IsValid)
            return Results.BadRequest(validationResult.Errors);

        var entry = entryDto.Adapt<Entry>();
        entry.Id = Guid.NewGuid();

        await unitOfWork.EntryRepository.InsertAsync(entry);

        return Results.Created($"/{Path}/{entry.Id}", entry.Adapt<EntryDetailedDto>());
    }

    internal static async Task<IResult> UpdateAsync(IUnitOfWork unitOfWork, Guid id, EntryDto entryDto, IValidator<EntryDto> validator)
    {
        if (id == Guid.Empty)
            return Results.BadRequest("Entry id is required.");

        if (entryDto == null)
            return Results.BadRequest("Entry data is required.");

        var validationResult = await validator.ValidateAsync(entryDto);

        if (!validationResult.IsValid)
            return Results.BadRequest(validationResult.Errors);

        var entry = entryDto.Adapt<Entry>();
        entry.Id = id;

        try
        {
            await unitOfWork.EntryRepository.UpdateAsync(entry);
        }
        catch (EntityNotFoundException<Entry> e)
        {
            return Results.NotFound(e.Message);
        }

        return Results.Ok(entry.Adapt<EntryDetailedDto>());
    }

    internal static async Task<IResult> DeleteAsync(IUnitOfWork unitOfWork, Guid id)
    {
        try
        {
            await unitOfWork.EntryRepository.DeleteAsync(id);
        }
        catch (EntityNotFoundException<Entry> e)
        {
            return Results.NotFound(e.Message);
        }

        return Results.NoContent();
    }
}
