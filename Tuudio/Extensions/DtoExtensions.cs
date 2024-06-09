using Tuudio.Domain.Entities.People;
using Tuudio.Models.DTOs;

namespace Tuudio.Extensions;

public static class DtoExtensions
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
}
