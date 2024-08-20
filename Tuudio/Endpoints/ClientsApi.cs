using FluentValidation;
using FluentValidation.Results;
using Mapster;
using Tuudio.Domain.Entities.People;
using Tuudio.Domain.Exceptions;
using Tuudio.DTOs.People;
using Tuudio.DTOs.People.Detailed;
using Tuudio.Infrastructure.Services.Interfaces;

namespace Tuudio.Endpoints;

public static class ClientsApi
{
    private const string Path = "clients";

    public static RouteGroupBuilder MapClientsApi(this RouteGroupBuilder group)
    {
        group
            .MapGet("/", async (IUnitOfWork uow) => await uow.ExecuteAsync(() => GetAsync(uow)))
            .Produces<IEnumerable<ClientDetailedDto>>(StatusCodes.Status200OK)
            .WithOpenApi();

        group
            .MapGet("/{id}", async (Guid id, IUnitOfWork uow) => await uow.ExecuteAsync(() => GetByIdAsync(uow, id)))
            .Produces<ClientDetailedDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi();

        group
            .MapPost("/", async (ClientDto clientDto, IValidator<ClientDto> validator, IUnitOfWork uow) => await uow.ExecuteAsync(() => AddAsync(uow, clientDto, validator)))
            .Produces<ClientDetailedDto>(StatusCodes.Status201Created)
            .Produces<IEnumerable<ValidationFailure>>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi();

        group
            .MapPut("/{id}", async (Guid id, ClientDto clientDto, IValidator<ClientDto> validator, IUnitOfWork uow) => await uow.ExecuteAsync(() => UpdateAsync(uow, id, clientDto, validator)))
            .Produces<ClientDetailedDto>(StatusCodes.Status200OK)
            .Produces<IEnumerable<ValidationFailure>>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi();

        group
            .MapDelete("/{id}", async (Guid id, IUnitOfWork uow) => await uow.ExecuteAsync(() => DeleteAsync(uow, id)))
            .Produces(StatusCodes.Status204NoContent)
            .Produces<string>(StatusCodes.Status404NotFound)
            .WithOpenApi();

        return group.WithTags("clients");
    }

    internal static async Task<IResult> GetAsync(IUnitOfWork unitOfWork)
    {
        var clients = await unitOfWork.ClientRepository.GetAllAsync();

        return Results.Ok(clients.Select(c => c.Adapt<ClientDetailedDto>()));
    }

    internal static async Task<IResult> GetByIdAsync(IUnitOfWork unitOfWork, Guid id)
    {
        var client = await unitOfWork.ClientRepository.GetByIdAsync(id);

        if (client == null)
            return Results.NotFound($"Client with ID \"{id}\" not found");

        return Results.Ok(client.Adapt<ClientDetailedDto>());
    }

    internal static async Task<IResult> AddAsync(IUnitOfWork unitOfWork, ClientDto clientDto, IValidator<ClientDto> validator)
    {
        if (clientDto == null)
            return Results.BadRequest("Client data is required.");

        var validationResult = await validator.ValidateAsync(clientDto);

        if (!validationResult.IsValid)
            return Results.BadRequest(validationResult.Errors);

        var client = clientDto.Adapt<Client>();
        client.Id = Guid.NewGuid();

        await unitOfWork.ClientRepository.InsertAsync(client);

        return Results.Created($"/{Path}/{client.Id}", client.Adapt<ClientDetailedDto>());
    }

    internal static async Task<IResult> UpdateAsync(IUnitOfWork unitOfWork, Guid id, ClientDto clientDto, IValidator<ClientDto> validator)
    {
        if (id == Guid.Empty)
            return Results.BadRequest("Client id is required.");

        if (clientDto == null)
            return Results.BadRequest("Client data is required.");

        var validationResult = await validator.ValidateAsync(clientDto);

        if (!validationResult.IsValid)
            return Results.BadRequest(validationResult.Errors);

        var client = clientDto.Adapt<Client>();
        client.Id = id;

        try
        {
            await unitOfWork.ClientRepository.UpdateAsync(client);
        }
        catch (EntityNotFoundException<Client> e)
        {
            return Results.NotFound(e.Message);
        }

        return Results.Ok(client.Adapt<ClientDetailedDto>());
    }

    internal static async Task<IResult> DeleteAsync(IUnitOfWork unitOfWork, Guid id)
    {
        try
        {
            await unitOfWork.ClientRepository.DeleteAsync(id);
        }
        catch (EntityNotFoundException<Client> e)
        {
            return Results.NotFound(e.Message);
        }

        return Results.NoContent();
    }
}
