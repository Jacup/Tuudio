using Tuudio.Interfaces;

namespace Tuudio.Models.DTOs;

public abstract record PersonDto : IDto
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }
}
