using FluentValidation;
using FluentValidation.Results;
using Tuudio.Domain.Exceptions;
using Tuudio.DTOs.People;
using Tuudio.DTOs.People.Detailed;
using Tuudio.Extensions.Clients;
using Tuudio.Infrastructure.Services.Interfaces;

namespace Tuudio.Endpoints;

public static class ClientsApi
{
    public static RouteGroupBuilder MapClientsApi(this RouteGroupBuilder group)
    {
        group
            .MapGet("/", async (IUnitOfWork uow) => await uow.ExecuteAsync(() => GetClientsAsync(uow)))
            .Produces<IEnumerable<ClientDetailedDto>>(StatusCodes.Status200OK)
            .WithOpenApi();

        group
            .MapGet("/{id}", async (Guid id, IUnitOfWork uow) => await uow.ExecuteAsync(() => GetClientByIdAsync(uow, id)))
            .Produces<ClientDetailedDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi();

        group
            .MapPost("/", async (ClientDto clientDto, IValidator<ClientDto> validator, IUnitOfWork uow) => await uow.ExecuteAsync(() => AddClientAsync(uow, clientDto, validator)))
            .Produces<ClientDetailedDto>(StatusCodes.Status201Created)
            .Produces<IEnumerable<ValidationFailure>>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi();

        group
            .MapPut("/{id}", async (Guid id, ClientDto clientDto, IValidator<ClientDto> validator, IUnitOfWork uow) => await uow.ExecuteAsync(() => UpdateClientAsync(uow, id, clientDto, validator)))
            .Produces<ClientDetailedDto>(StatusCodes.Status200OK)
            .Produces<IEnumerable<ValidationFailure>>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi();

        group
            .MapDelete("/{id}", async (Guid id, IUnitOfWork uow) => await uow.ExecuteAsync(() => DeleteClientAsync(uow, id)))
            .Produces(StatusCodes.Status204NoContent)
            .Produces<string>(StatusCodes.Status404NotFound)
            .WithOpenApi();

        return group;
    }

    internal static async Task<IResult> GetClientsAsync(IUnitOfWork unitOfWork)
    {
        var clients = await unitOfWork.ClientRepository.GetAllAsync();

        return Results.Ok(clients.Select(c => c.ToDetailedDto()));
    }

    internal static async Task<IResult> GetClientByIdAsync(IUnitOfWork unitOfWork, Guid id)
    {
        var client = await unitOfWork.ClientRepository.GetByIdAsync(id);

        if (client == null)
            return Results.NotFound($"Client with ID \"{id}\" not found");

        return Results.Ok(client.ToDetailedDto());
    }

    internal static async Task<IResult> AddClientAsync(IUnitOfWork unitOfWork, ClientDto clientDto, IValidator<ClientDto> validator)
    {
        if (clientDto == null)
            return Results.BadRequest("Client data is required.");

        var validationResult = await validator.ValidateAsync(clientDto);

        if (!validationResult.IsValid)
            return Results.BadRequest(validationResult.Errors);

        var client = clientDto.FromDto();

        await unitOfWork.ClientRepository.InsertAsync(client);

        return Results.Created($"/clients/{client.Id}", client.ToDetailedDto());
    }

    internal static async Task<IResult> UpdateClientAsync(IUnitOfWork unitOfWork, Guid id, ClientDto clientDto, IValidator<ClientDto> validator)
    {
        if (id == Guid.Empty)
            return Results.BadRequest("Client id is required.");

        if (clientDto == null)
            return Results.BadRequest("Client data is required.");

        var validationResult = await validator.ValidateAsync(clientDto);

        if (!validationResult.IsValid)
            return Results.BadRequest(validationResult.Errors);

        var client = clientDto.FromDto(id);

        try
        {
            await unitOfWork.ClientRepository.UpdateAsync(client);
        }
        catch (ClientNotFoundException e)
        {
            return Results.NotFound(e.Message);
        }

        return Results.Ok(client.ToDetailedDto());
    }

    internal static async Task<IResult> DeleteClientAsync(IUnitOfWork unitOfWork, Guid id)
    {
        try
        {
            await unitOfWork.ClientRepository.DeleteAsync(id);
        }
        catch (ClientNotFoundException e)
        {
            return Results.NotFound(e.Message);
        }

        return Results.NoContent();
    }
}
