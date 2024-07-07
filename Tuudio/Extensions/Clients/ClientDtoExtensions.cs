using Tuudio.Domain.Entities.People;
using Tuudio.DTOs.People;
using Tuudio.DTOs.People.Detailed;

namespace Tuudio.Extensions.Clients;

public static class ClientDtoExtensions
{
    public static Client FromDto(this ClientDto dto) => dto.FromDto(Guid.NewGuid());

    public static Client FromDto(this ClientDto dto, Guid id) => new()
    {
        Id = id,

        FirstName = dto.FirstName,
        LastName = dto.LastName,
        Email = dto.Email,
        PhoneNumber = dto.PhoneNumber,
    };

    public static ClientDto ToDto(this Client client) => new()
    {
        FirstName = client.FirstName,
        LastName = client.LastName,
        Email = client.Email,
        PhoneNumber = client.PhoneNumber,
    };

    public static ClientDetailedDto ToDetailedDto(this Client client) => new()
    {
        Id = client.Id,

        CreatedDate = client.CreatedDate,
        UpdatedDate = client.UpdatedDate,

        FirstName = client.FirstName,
        LastName = client.LastName,
        Email = client.Email,
        PhoneNumber = client.PhoneNumber,
    };
}
